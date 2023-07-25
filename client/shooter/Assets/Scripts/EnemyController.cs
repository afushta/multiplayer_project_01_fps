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

    public void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    position.x = (float)change.Value;
                    break;
                case "y":
                    position.z = (float)change.Value;
                    break;
                default:
                    Debug.LogWarning($"Changes to the {change.Field} parameter are not processed");
                    break;
            }
        }

        _enemy.SetPosition(position);
    }
}
