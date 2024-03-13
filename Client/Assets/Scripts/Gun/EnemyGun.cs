using System;
using UnityEngine;

namespace Gun
{
    public class EnemyGun : Gun
    {
        public override event Action OnShoot;

        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            bullet.Init(velocity);
            OnShoot?.Invoke();
        }
    }
}