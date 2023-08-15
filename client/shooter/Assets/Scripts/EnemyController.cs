using Colyseus.Schema;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum PlayerField { Position, Velocity, Rotation, AngularVelocity }


[RequireComponent(typeof(EnemyCharacter))]
public class EnemyController : MonoBehaviour
{
    private EnemyCharacter _enemy;
    [SerializeField] private EnemyGun _gun;
    private ReceiveTimeInterval _receiveTimeIntervals;

    private void Awake()
    {
        _enemy = GetComponent<EnemyCharacter>();
        _receiveTimeIntervals = new ReceiveTimeInterval(Time.time);
    }

    public void Init(Player player)
    {
        _enemy.SetMaxSpeed(player.speed);
        player.position.OnChange += OnPositionChange;
        player.velocity.OnChange += OnVelocityChange;
        player.rotation.OnChange += OnRotationChange;
        player.angularVelocity.OnChange += OnAngularVelocityChange;
        player.OnChange += OnChange;
        player.OnRemove += OnRemove;
    }

    public void Shoot(in ShootInfo shootInfo)
    {
        Vector3 position = new Vector3(shootInfo.pX, shootInfo.pY, shootInfo.pZ);
        Vector3 velocity = new Vector3(shootInfo.vX, shootInfo.vY, shootInfo.vZ);

        _gun.Shoot(position, velocity, _receiveTimeIntervals.AverageValue);
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

    public void OnVector3FieldChange(PlayerField field, Vector3 initialValue, List<DataChange> changes)
    {
        _receiveTimeIntervals.Update(field);
        Vector3 newValue = ProcessVector3Changes(initialValue, changes);
        switch (field)
        {
            case PlayerField.Position:
                _enemy.UpdatePosition(newValue, _receiveTimeIntervals.AverageValue);
                break;
            case PlayerField.Velocity:
                _enemy.UpdateVelocity(newValue, _receiveTimeIntervals.AverageValue);
                break;
            case PlayerField.Rotation:
                _enemy.UpdateRotation(newValue, _receiveTimeIntervals.AverageValue);
                break;
            case PlayerField.AngularVelocity:
                _enemy.UpdateAngularVelocity(newValue, _receiveTimeIntervals.AverageValue);
                break;
            default:
                break;
        }
    }

    public void OnPositionChange(List<DataChange> changes)
    {
        OnVector3FieldChange(PlayerField.Position, _enemy.TargetPosition, changes);
    }

    public void OnVelocityChange(List<DataChange> changes)
    {
        OnVector3FieldChange(PlayerField.Velocity, _enemy.Velocity, changes);
    }

    public void OnRotationChange(List<DataChange> changes)
    {
        OnVector3FieldChange(PlayerField.Rotation, _enemy.TargetRotation, changes);
    }

    public void OnAngularVelocityChange(List<DataChange> changes)
    {
        OnVector3FieldChange(PlayerField.AngularVelocity, _enemy.AngularVelocity, changes);
    }

    public void OnChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "isCrouching":
                    _enemy.UpdateCrouch((bool)change.Value);
                    break;
                default:
                    Debug.LogWarning($"Get changes in unsupported field {change.Field}");
                    break;
            }
        }
    }

    public void OnRemove()
    {
        Debug.Log("Enemy disconnected from server");
        Destroy(gameObject);
    }
}


class ReceiveTimeInterval
{
    private float[] _receiveTimeIntervals = new float[5] { 0f, 0f, 0f, 0f, 0f };
    private int _receiveTimeIntervalsPointer;
    private Dictionary<PlayerField, float> _receiveTimer;
    
    public float AverageValue => _receiveTimeIntervals.Average();

    public ReceiveTimeInterval(float value)
    {
        _receiveTimer = new Dictionary<PlayerField, float>()
        {
            { PlayerField.Position, value },
            { PlayerField.Velocity, value },
            { PlayerField.Rotation, value },
            { PlayerField.AngularVelocity, value },
        };
    }

    public void Update(PlayerField timerType)
    {
        _receiveTimeIntervals[_receiveTimeIntervalsPointer] = Time.time - _receiveTimer[timerType];
        _receiveTimer[timerType] = Time.time;
        _receiveTimeIntervalsPointer = (_receiveTimeIntervalsPointer + 1) % _receiveTimeIntervals.Length;
    }
}
