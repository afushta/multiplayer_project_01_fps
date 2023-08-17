using System.Collections.Generic;
using UnityEngine;

public class ScoreTableUI : MonoBehaviour
{
    [SerializeField] private PlayerScoreUI _playerScoreUIPrefab;

    private Dictionary<string, PlayerScoreUI> playerScoreUI = new Dictionary<string, PlayerScoreUI>();

    public void AddPlayer(string playerId, string playerName)
    {
        playerScoreUI[playerId] = Instantiate(_playerScoreUIPrefab, transform);
        playerScoreUI[playerId].Init(playerName);
    }

    public void UpdatePlayerScore(string playerId, Score score)
    {
        playerScoreUI[playerId].UpdateScore(score);
    }
}
