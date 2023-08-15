using System;
using UnityEngine;


public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] private AudioSource _shotAudio;
    public Action OnShoot;

    private void Awake()
    {
        OnShoot += _shotAudio.Play;
    }
}
