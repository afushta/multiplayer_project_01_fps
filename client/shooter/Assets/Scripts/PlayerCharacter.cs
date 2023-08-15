using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(CheckGrounded))]
public class PlayerCharacter : BaseCharacter
{
    private const string CROUCH = "Crouch";

    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpDelay = 0.2f;

    private float _jumpTimer;

    private Rigidbody _rigidbody;
    private CheckGrounded _checkGrounded;

    private float _movementH;
    private float _movementV;
    private float _rotationH;
    private float _rotationV;
    private bool _isCrouching;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _checkGrounded = GetComponent<CheckGrounded>();

        Transform cameraTransform = Camera.main.transform;
        cameraTransform.parent = _cameraPoint;
        cameraTransform.localPosition = Vector3.zero;
        cameraTransform.localRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Move();
        RotateH();
    }

    private void Move()
    {
        Vector3 direction = (transform.forward * _movementV + transform.right * _movementH).normalized;
        Vector3 yVelocity = transform.up * _rigidbody.velocity.y;
        Velocity = direction * MaxSpeed + yVelocity;
        _rigidbody.velocity = Velocity;
    }

    public void Jump()
    {
        if (!_checkGrounded.IsGrounded) return;

        if (Time.time - _jumpTimer > _jumpDelay)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            _jumpTimer = Time.time;
        }
    }

    public void Crouch(bool isActive)
    {
        _isCrouching = isActive;
        _characterAnimator.SetBool(CROUCH, isActive);
    }

    public void RotateV(float value)
    {
        _rotationV = Mathf.Clamp(_rotationV + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_rotationV, 0f, 0f);
    }

    private void RotateH()
    {
        _rigidbody.angularVelocity = new Vector3(0f, _rotationH, 0f);
        _rotationH = 0f;
    }

    public void SetInput(float h, float v, float rotationH)
    {
        _movementH = h;
        _movementV = v;
        _rotationH += rotationH;
    }

    public void GetMovementInfo(out Vector3 position, out Vector3 velocity, out Vector3 rotation, out float angularVelocity, out bool isCrouching)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotation = new Vector3(_rotationV, transform.eulerAngles.y, 0f);
        angularVelocity = _rigidbody.angularVelocity.y;
        isCrouching = _isCrouching;
    }
}
