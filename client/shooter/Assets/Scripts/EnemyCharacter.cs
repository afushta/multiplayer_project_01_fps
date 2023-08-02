using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private float lerpStrength = 10f;

    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        targetPosition += currentVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, lerpStrength * Time.deltaTime);
    }

    public void UpdatePosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void UpdateVelocity(Vector3 velocity)
    {
        currentVelocity = velocity;
    }
}
