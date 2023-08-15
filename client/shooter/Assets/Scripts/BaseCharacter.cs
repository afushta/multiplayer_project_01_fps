using UnityEngine;


public abstract class BaseCharacter : MonoBehaviour
{
    [field: SerializeField] public float MaxSpeed { get; protected set; } = 2f;
    public Vector3 Velocity { get; protected set; }
}
