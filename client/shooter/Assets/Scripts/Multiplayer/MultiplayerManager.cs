using Colyseus;
using System;
using System.Collections.Generic;
using UnityEngine;


public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private EnemyController _enemyPrefab;

    private ColyseusRoom<State> _room;
    private string _sessionId => _room?.SessionId;

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;

        state.players.ForEach(
            (playerId, player) =>
            {
                if (playerId == _sessionId) CreatePlayer(player);
                else CreateEnemy(playerId, player);
            }
        );

        state.players.OnAdd += CreateEnemy;
        state.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        Vector3 position = new Vector3(player.x, 0f, player.y);
        Instantiate(_playerPrefab, position, Quaternion.identity);
    }

    private void CreateEnemy(string playerId, Player player)
    {
        Vector3 position = new Vector3(player.x, 0f, player.y);
        EnemyController enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        player.OnChange += enemy.OnChange;
    }

    private void RemoveEnemy(string key, Player value)
    {
        Debug.Log($"Enemy {key} disconnected from server");
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room?.Leave();
    }
}
