using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    
    private float _inputH;
    private float _inputV;
    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_inputH, 0f, _inputV).normalized;
        transform.Translate(direction * Time.deltaTime * _speed);
    }

    public void SetInput(float h, float v)
    {
        _inputH = h;
        _inputV = v;
    }
}
