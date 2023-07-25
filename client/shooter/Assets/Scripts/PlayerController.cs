using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _player;

    private void Start()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        _player.SetInput(inputH, inputV);
    }

    private void LateUpdate()
    {
        SendChanges();
    }

    private void SendChanges()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "x", transform.position.x },
            { "y", transform.position.z }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
