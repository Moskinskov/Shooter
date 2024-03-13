using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string groundedKey = "Grounded";
    private const string speedKey = "Speed";

    [SerializeField] private Animator animator;
    [SerializeField] private CheckFly checkFly;
    [SerializeField] private Character character;

    private void Update()
    {
        Vector3 localVelocity = character.transform.InverseTransformVector(character.Velocity);
        float speed = localVelocity.magnitude / character.Speed;
        float sign = Mathf.Sign(localVelocity.z);

        animator.SetBool(groundedKey, checkFly.IsFly is false);
        animator.SetFloat(speedKey, speed * sign);
    }
}