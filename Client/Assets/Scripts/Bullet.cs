using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    private int damage = default;
    private float delayDestroy = 3f;

    public void Init(Vector3 velocity, int damage = 0)
    {
        this.damage = damage;
        rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(delayDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(damage);
        }
    }
}