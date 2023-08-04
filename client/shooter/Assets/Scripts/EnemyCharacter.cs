using UnityEngine;

public class EnemyCharacter : BaseCharacter
{
    [SerializeField] private Transform _head;

    private Vector3 serverPosition;
    private Vector3 serverRotation;

    public Vector3 TargetPosition { get; private set; }
    public Vector3 TargetRotation { get; private set; }
    public Vector3 AngularVelocity { get; private set; }

    private void Awake()
    {
        TargetPosition = transform.position;
        TargetRotation = new Vector3(_head.localEulerAngles.x, transform.eulerAngles.y, 0f);
    }

    private void Update()
    {
        if (Velocity.magnitude > 0.01f)
        {
            float maxDistance = Velocity.magnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = TargetPosition;
        }

        Quaternion headTargetRotation = Quaternion.Euler(TargetRotation.x, 0f, 0f);
        Quaternion bodyTargetRotation = Quaternion.Euler(0f, TargetRotation.y, 0f);

        if (AngularVelocity.magnitude > 0.01f)
        {
            float maxRotationX = AngularVelocity.x * Time.deltaTime;
            float maxRotationY = AngularVelocity.y * Time.deltaTime;
            _head.localRotation = Quaternion.RotateTowards(_head.localRotation, headTargetRotation, maxRotationX);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, bodyTargetRotation, maxRotationY);
        }
        else
        {
            _head.localRotation = headTargetRotation;
            transform.rotation = bodyTargetRotation;
        }
    }

    public void SetMaxSpeed(float value)
    {
        MaxSpeed = value;
    }

    private void UpdateTargetPosition(float averageReceiveTimeInterval)
    {
        TargetPosition = serverPosition + Velocity * averageReceiveTimeInterval;
    }

    private void UpdateTargetRotation(float averageReceiveTimeInterval)
    {
        TargetRotation = serverRotation + AngularVelocity * averageReceiveTimeInterval;
    }

    public void UpdatePosition(Vector3 position, float averageReceiveTimeInterval)
    {
        serverPosition = position;
        UpdateTargetPosition(averageReceiveTimeInterval);
    }

    public void UpdateVelocity(Vector3 velocity, float averageReceiveTimeInterval)
    {
        Velocity = velocity;
        UpdateTargetPosition(averageReceiveTimeInterval);
    }

    public void UpdateRotation(Vector3 rotation, float averageReceiveTimeInterval)
    {
        serverRotation = rotation;
        UpdateTargetRotation(averageReceiveTimeInterval);
    }

    public void UpdateAngularVelocity(Vector3 angularVelocity, float averageReceiveTimeInterval)
    {
        AngularVelocity = angularVelocity;
        UpdateTargetRotation(averageReceiveTimeInterval);
    }
}
