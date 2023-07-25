using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyCharacter))]
public class EnemyController : MonoBehaviour
{
    private EnemyCharacter _enemy;

    private void Start()
    {
        _enemy = GetComponent<EnemyCharacter>();
    }

    private Vector3 ProcessVector3Changes(Vector3 value, List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    value.x = (float)change.Value;
                    break;
                case "y":
                    value.y = (float)change.Value;
                    break;
                case "z":
                    value.z = (float)change.Value;
                    break;
                default:
                    Debug.LogWarning($"Changes to the {change.Field} parameter are not processed");
                    break;
            }
        }

        return value;
    }

    public void OnPositionChange(List<DataChange> changes)
    {
        Vector3 newPosition = ProcessVector3Changes(transform.position, changes);
        _enemy.UpdatePosition(newPosition);
    }

    public void OnVelocityChange(List<DataChange> changes)
    {
        Vector3 newVelocity = ProcessVector3Changes(Vector3.zero, changes);
        _enemy.UpdateVelocity(newVelocity);
    }
}
