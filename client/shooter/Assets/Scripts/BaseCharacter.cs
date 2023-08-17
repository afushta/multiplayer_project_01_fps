using UnityEngine;


[RequireComponent(typeof(Health))]
public abstract class BaseCharacter : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; protected set; } = 10;
    [field: SerializeField] public float MaxSpeed { get; protected set; } = 2f;
    public Vector3 Velocity { get; protected set; }

    protected Health _health;

    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _health.SetMax(MaxHealth);
        _health.SetCurrent(MaxHealth);
    }
}
