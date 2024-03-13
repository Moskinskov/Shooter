using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float Speed { get; protected set; } = 5f;
    public Vector3 Velocity { get; set; }
}