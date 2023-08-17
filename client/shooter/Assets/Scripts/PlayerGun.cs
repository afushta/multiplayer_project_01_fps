using System;
using UnityEngine;


public class PlayerGun : Gun
{
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private int _bulletDamage = 1;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootDelay = 0.2f;

    private float _lastShootTime;

    public bool TryShoot(out ShootInfo shootInfo)
    {
        shootInfo = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;
        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity, _bulletDamage);
        OnShoot?.Invoke();
        
        shootInfo.pX = position.x;
        shootInfo.pY = position.y;
        shootInfo.pZ = position.z;
        shootInfo.vX = velocity.x;
        shootInfo.vY = velocity.y;
        shootInfo.vZ = velocity.z;

        return true;
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float vX;
    public float vY;
    public float vZ;
}
