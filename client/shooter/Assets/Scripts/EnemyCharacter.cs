using UnityEngine;

public class EnemyCharacter : BaseCharacter
{
    private const string CROUCH = "Crouch";

    [SerializeField] private Transform _head;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private float _lerpStrength = 5f;

    private Vector3 _serverPosition;
    private Vector3 _serverRotation;

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
        Move();
        Rotate();
    }

    private void Move()
    {
        if (Velocity.magnitude > 0.01f)
        {
            float maxDistance = Velocity.magnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, _lerpStrength * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        Quaternion headTargetRotation = Quaternion.Euler(TargetRotation.x, 0f, 0f);
        if (AngularVelocity.x > 0.01f)
        {
            float maxRotationX = AngularVelocity.x * Time.deltaTime;
            _head.localRotation = Quaternion.RotateTowards(_head.localRotation, headTargetRotation, maxRotationX);
        }
        else
        {
            _head.localRotation = Quaternion.Lerp(_head.localRotation, headTargetRotation, _lerpStrength * Time.deltaTime);
        }

        Quaternion bodyTargetRotation = Quaternion.Euler(0f, TargetRotation.y, 0f);
        if (AngularVelocity.y > 0.01f)
        {
            float maxRotationY = AngularVelocity.y * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, bodyTargetRotation, maxRotationY);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, bodyTargetRotation, _lerpStrength * Time.deltaTime);
        }
    }

    public void SetMaxSpeed(float value)
    {
        MaxSpeed = value;
    }

    private void UpdateTargetPosition(float averageReceiveTimeInterval)
    {
        TargetPosition = _serverPosition + Velocity * averageReceiveTimeInterval;
    }

    private void UpdateTargetRotation(float averageReceiveTimeInterval)
    {
        TargetRotation = _serverRotation + AngularVelocity * averageReceiveTimeInterval;
    }

    public void UpdatePosition(Vector3 position, float averageReceiveTimeInterval)
    {
        _serverPosition = position;
        UpdateTargetPosition(averageReceiveTimeInterval);
    }

    public void UpdateVelocity(Vector3 velocity, float averageReceiveTimeInterval)
    {
        Velocity = velocity;
        UpdateTargetPosition(averageReceiveTimeInterval);
    }

    public void UpdateRotation(Vector3 rotation, float averageReceiveTimeInterval)
    {
        _serverRotation = rotation;
        UpdateTargetRotation(averageReceiveTimeInterval);
    }

    public void UpdateAngularVelocity(Vector3 angularVelocity, float averageReceiveTimeInterval)
    {
        AngularVelocity = angularVelocity;
        UpdateTargetRotation(averageReceiveTimeInterval);
    }

    public void UpdateCrouch(bool value)
    {
        _characterAnimator.SetBool(CROUCH, value);
    }
}
