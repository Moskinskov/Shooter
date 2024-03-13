using System;
using UnityEngine;

namespace Gun
{
    public class PlayerGun : Gun
    {
        [SerializeField] private Transform bulletPoint;
        [SerializeField] private float bulletSpeed;

        private float shootDelay = 0.5f;
        private float lastShootTime = 0f;

        public override event Action OnShoot;

        public bool TryShoot(out ShootInfo info)
        {
            info = new ShootInfo();

            if (Time.time - lastShootTime < shootDelay) return false;

            lastShootTime = Time.time;
            Vector3 position = bulletPoint.position;
            Vector3 direction = bulletPoint.forward * bulletSpeed;

            Bullet bullet = Instantiate(bulletPrefab, position, bulletPoint.rotation);
            bullet.Init(direction);

            OnShoot?.Invoke();

            info.pX = position.x;
            info.pY = position.y;
            info.pZ = position.z;
            info.dX = direction.x;
            info.dY = direction.y;
            info.dZ = direction.z;

            return true;
        }
    }
}