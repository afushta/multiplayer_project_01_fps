using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private Vector2 _mouseSensetivity = new Vector2(1f, 1f);

    private PlayerCharacter _player;
    private MultiplayerManager _multiplayerManager;

    private void Start()
    {
        _player = GetComponent<PlayerCharacter>();
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
        if (isShooting && _playerGun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);
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
}
