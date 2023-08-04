using UnityEngine;


[RequireComponent(typeof(CheckGrounded))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _footAnimator;

    private BaseCharacter _character;
    private CheckGrounded _checkGrounded;

    private void Start()
    {
        _character = GetComponent<BaseCharacter>();
        _checkGrounded = GetComponent<CheckGrounded>();
    }

    private void Update()
    {
        Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        float speed = localVelocity.magnitude / _character.MaxSpeed;
        float sign = Mathf.Sign(localVelocity.z);

        _footAnimator.SetBool("Grounded", _checkGrounded.IsGrounded);
        _footAnimator.SetFloat("Speed", speed * sign);
    }
}
