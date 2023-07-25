using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _player;
    private float inputH;
    private float inputV;

    private void Start()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        _player.SetInput(inputH, inputV);
    }

    private void LateUpdate()
    {
        SendChanges();
    }

    private void SendChanges()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "position", new Dictionary<string, float>()
                {
                    { "x", transform.position.x },
                    { "y", transform.position.y },
                    { "z", transform.position.z }
                } 
            },
            { "velocity", new Dictionary<string, float>()
                {
                    { "x", inputH },
                    { "z", inputV }
                }
            },
            { "rotation", new Dictionary<string, float>()
                {
                    { "x", rotation.x },
                    { "y", rotation.y },
                    { "z", rotation.z }
                }
            }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
