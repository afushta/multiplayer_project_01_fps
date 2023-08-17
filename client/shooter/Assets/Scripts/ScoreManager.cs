using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreTableUI scoreTableUI;

    public static ScoreManager Instance { get; private set; }

    private Dictionary<string, Score> scoreDict = new Dictionary<string, Score>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        scoreTableUI.gameObject.SetActive(Input.GetKey(KeyCode.Tab));
    }

    public void AddPlayer(string playerId, string playerName)
    {
        scoreDict[playerId] = new Score();
        scoreTableUI.AddPlayer(playerId, playerName);
    }

    public void UpdateKills(string playerId, int value)
    {
        scoreDict[playerId].kills = value;
        UpdateUI(playerId);
    }

    public void UpdateDeaths(string playerId, int value)
    {
        scoreDict[playerId].deaths = value;
        UpdateUI(playerId);
    }

    private void UpdateUI(string playerId)
    {
        scoreTableUI.UpdatePlayerScore(playerId, scoreDict[playerId]);
    }
}

public class Score
{
    public int kills;
    public int deaths;
}
