using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Gun.Gun playerGun;
    [SerializeField] private Animator animator;
    private const string shootTriggerKey = "TryShoot";

    private void Start()
    {
        playerGun.OnShoot += OnPlayerGunShootHandler;
    }

    private void OnPlayerGunShootHandler()
    {
        animator.SetTrigger(shootTriggerKey);
    }

    private void OnDestroy()
    {
        if (playerGun != null) playerGun.OnShoot -= OnPlayerGunShootHandler;
    }
}