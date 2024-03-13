using UnityEngine;

public class CheckFly : MonoBehaviour
{
    [SerializeField] private float radius = 0.2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float coyoteTime = 0.15f;

    private float flyTimer = 0f;
    public bool IsFly { get; set; }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, layerMask))
        {
            IsFly = false;
            flyTimer = 0f;
        }
        else
        {
            flyTimer += Time.deltaTime;
            if (flyTimer >= coyoteTime) IsFly = true;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}