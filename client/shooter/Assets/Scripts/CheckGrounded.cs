using UnityEngine;


public class CheckGrounded : MonoBehaviour
{
    [SerializeField] private float _radius = 0.2f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _coyoteTime = 0.15f;
    private float _flyTimer = 0f;

    public bool IsGrounded { get; private set; }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            IsGrounded = true;
            _flyTimer = 0f;
        }
        else
        {
            _flyTimer += Time.deltaTime;
            if (_flyTimer > _coyoteTime) IsGrounded = false;
        }
    }
}
