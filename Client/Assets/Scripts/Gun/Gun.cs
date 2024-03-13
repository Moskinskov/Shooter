using System;
using UnityEngine;

namespace Gun
{
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Bullet bulletPrefab;
        public abstract event Action OnShoot;
    }
}