using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int MaxHealth { get; set; } = 10;
    public float Speed { get; protected set; } = 5f;
    public Vector3 Velocity { get; set; }
}