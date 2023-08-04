using UnityEngine;


public class GunAnimation : MonoBehaviour
{
    private const string SHOOT = "Shoot";

    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private Gun _gun;

    private void Start()
    {
        _gun.OnShoot += Shoot;
    }

    private void Shoot()
    {
        _gunAnimator.SetTrigger(SHOOT);
    }

    private void OnDestroy()
    {
        _gun.OnShoot -= Shoot;
    }
}
