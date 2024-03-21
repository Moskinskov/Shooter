using System;
using UnityEngine;

namespace Gun
{
    public class GunRay : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform center;
        [SerializeField] private Transform rayPoint;
        [SerializeField] private float rayPointSize;

        private Transform camera;

        private void Awake()
        {
            camera = Camera.main.transform;
        }

        private void Update()
        {
            Ray ray = new Ray(center.position, center.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 50f, layerMask, QueryTriggerInteraction.Ignore))
            {
                center.localScale = new Vector3(1, 1, hit.distance);
                rayPoint.position = hit.point;
                float distance = Vector3.Distance(camera.position, hit.point);
                rayPoint.localScale = Vector3.one * distance * rayPointSize;
            }
        }
    }
}