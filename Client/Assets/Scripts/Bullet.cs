using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    private float delayDestroy = 3f;

    public void Init(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(delayDestroy);
        Destroy(gameObject);
    }
}