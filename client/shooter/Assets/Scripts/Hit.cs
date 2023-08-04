using System.Collections;
using UnityEngine;


public class Hit : MonoBehaviour
{
    [SerializeField] private float _ttl = 1f;

    private void Start()
    {
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
}
