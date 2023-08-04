using UnityEngine;


public class EnemyGun : Gun
{
    public void Shoot(Vector3 position, Vector3 velocity, float _averageTimeInterval)
    {
        Vector3 predictedPosition = position + velocity * _averageTimeInterval;

        Instantiate(_bulletPrefab, predictedPosition, Quaternion.identity).Init(velocity);
        OnShoot?.Invoke();
    }
}
