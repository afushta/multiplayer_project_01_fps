using Colyseus.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private Inventory<PlayerGun> _playerInventory;
    [SerializeField] private Vector2 _mouseSensetivity = new Vector2(1f, 1f);
    [SerializeField] private PlayerCharacter _player;
    
    private MultiplayerManager _multiplayerManager;

    public int MaxHealth => _player.MaxHealth;
    public float MaxSpeed => _player.MaxSpeed;

    public void Init(Player player)
    {
        ScoreManager.Instance.AddPlayer(_multiplayerManager.SessionId, "Player");
        player.OnChange += OnChange;
    }

    private void OnChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "deaths":
                    ScoreManager.Instance.UpdateDeaths(_multiplayerManager.SessionId, (byte)change.Value);
                    break;
                case "kills":
                    ScoreManager.Instance.UpdateKills(_multiplayerManager.SessionId, (byte)change.Value);
                    break;
                case "currentHP":
                    _player.UpdateHealth((sbyte)change.Value);
                    break;
                default:
                    Debug.LogWarning($"Get changes in unsupported field {change.Field}");
                    break;
            }
        }
    }

    private void Awake()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool isJumping = Input.GetKeyDown(KeyCode.Space);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        bool isShooting = Input.GetMouseButton(0);

        _player.RotateV(-mouseY * _mouseSensetivity.y);
        _player.SetInput(inputH, inputV, mouseX * _mouseSensetivity.x);

        if (isJumping) _player.Jump();
        _player.Crouch(isCrouching);
        if (isShooting && _playerInventory.CurrentGun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchGun(1);
    }

    private void SwitchGun(int index)
    {
        _playerInventory.SwitchGun(index);

        Dictionary<string, object> data = new()
        {
            { "gunId", index }
        };
        _multiplayerManager.SendMessage("gun", data);
    }

    private void LateUpdate()
    {
        SendChanges();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = MultiplayerManager.Instance.SessionId;
        string data = JsonUtility.ToJson(shootInfo);
        _multiplayerManager.SendMessage("shoot", data);
    }

    private void SendChanges()
    {
        _player.GetMovementInfo(out Vector3 position, out Vector3 velocity, out Vector3 rotation, out float angularVelocity, out bool isCrouching);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "position", new Dictionary<string, float>()
                {
                    { "x", position.x },
                    { "y", position.y },
                    { "z", position.z }
                } 
            },
            { "velocity", new Dictionary<string, float>()
                {
                    { "x", velocity.x },
                    { "y", velocity.y },
                    { "z", velocity.z }
                }
            },
            { "rotation", new Dictionary<string, float>()
                {
                    { "x", rotation.x },
                    { "y", rotation.y }
                }
            },
            { "angularVelocity", new Dictionary<string, float>()
                {
                    { "x", 0 },
                    { "y", angularVelocity }
                }
            },
            { "isCrouching", isCrouching }
        };

        _multiplayerManager.SendMessage("move", data);
    }

    public void Respawn(string json)
    {
        Vector3 newPosition = JsonUtility.FromJson<Vector3>(json);
        transform.position = newPosition;
        _player.SetInput(0, 0, 0);
        SendChanges();
        StartCoroutine(Hold());
    }

    private IEnumerator Hold()
    {
        yield return new WaitForSecondsRealtime(_respawnDelay);
    }
}
