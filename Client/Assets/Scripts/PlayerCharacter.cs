using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Rigidbody rigidbody;

    private float inputH;
    private float inputV;

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * inputV + transform.right * inputH).normalized * speed;
        rigidbody.velocity = velocity;
    }

    public void SetInput(float h, float v)
    {
        inputH = h;
        inputV = v;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = rigidbody.velocity;
    }
}