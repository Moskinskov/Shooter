using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform head;
    [SerializeField] private float minHeadAngle = -90;
    [SerializeField] private float maxHeadAngle = 90;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private CheckFly checkFly;

    private float inputH = 0f;
    private float inputV = 0f;
    private float rotateY = 0f;
    private float currentRotateX = 0f;
    private float jumpTime = 0f;
    private float jumpDelay = 0.2f;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.SetParent(cameraPoint);
        camera.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * inputV + transform.right * inputH).normalized * Speed;
        velocity.y = rigidbody.velocity.y;
        Velocity = velocity;
        rigidbody.velocity = Velocity;
    }

    public void SetInput(float h, float v, float rotateY)
    {
        inputH = h;
        inputV = v;
        this.rotateY += rotateY;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = rigidbody.velocity;
        rotateX = head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }

    private void RotateY()
    {
        rigidbody.angularVelocity = new Vector3(0f, rotateY, 0f);
        rotateY = 0f;
    }

    public void RotateX(float value)
    {
        currentRotateX = Mathf.Clamp((currentRotateX + value), minHeadAngle, maxHeadAngle);
        head.localEulerAngles = new Vector3(currentRotateX, 0f, 0f);
    }

    public void Jump()
    {
        if (checkFly.IsFly) return;
        if (Time.time - jumpTime < jumpDelay) return;

        jumpTime = Time.time;
        rigidbody.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);
    }
}