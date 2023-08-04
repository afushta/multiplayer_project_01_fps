using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _ttl = 3f;
    [SerializeField] private Hit _hitPrefab;

    public void Init(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
        StartCoroutine(nameof(DestroyDelay));
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSecondsRealtime(_ttl);
        SelfDestroy();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Instantiate(_hitPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        SelfDestroy();
    }
}
