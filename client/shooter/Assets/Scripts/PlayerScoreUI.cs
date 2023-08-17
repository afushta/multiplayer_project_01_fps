using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text kills;
    [SerializeField] private TMP_Text deaths;

    public void Init(string name)
    {
        playerName.text = name;
    }

    public void UpdateScore(Score score)
    {
        kills.text = score.kills.ToString();
        deaths.text = score.deaths.ToString();
    }
}
