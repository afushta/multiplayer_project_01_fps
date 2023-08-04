using Colyseus;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private PlayerCharacter _playerPrefab;
    [SerializeField] private EnemyController _enemyPrefab;

    private ColyseusRoom<State> _room;
    public string SessionId => _room?.SessionId;

    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        Dictionary<string, object> options = new Dictionary<string, object>()
        {
            { "speed", _playerPrefab.MaxSpeed }
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
        _room.OnStateChange += OnChange;
        _room.OnMessage<string>("shoot", OnEnemyShoot);
    }

    

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;

        state.players.ForEach(
            (playerId, player) =>
            {
                if (playerId == SessionId) CreatePlayer(player);
                else CreateEnemy(playerId, player);
            }
        );

        state.players.OnAdd += CreateEnemy;
        state.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.position.x, player.position.y, player.position.z);
        Instantiate(_playerPrefab, position, Quaternion.identity);
    }

    private void CreateEnemy(string playerId, Player player)
    {
        Vector3 position = new Vector3(player.position.x, player.position.y, player.position.z);
        EnemyController enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.Init(player);
        _enemies.Add(playerId, enemy);
    }

    private void RemoveEnemy(string playerId, Player player)
    {
        _enemies.Remove(playerId);
    }

    private void OnEnemyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        if (_enemies.TryGetValue(shootInfo.key, out EnemyController enemy))
        {
            enemy.Shoot(in shootInfo);
        }
        else
        {
            Debug.LogError($"Receive ShootInfo from unknown enemy {shootInfo.key}");
        }
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room?.Leave();
    }
}
