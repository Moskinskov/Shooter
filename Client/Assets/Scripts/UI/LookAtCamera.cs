using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            target.LookAt(Camera.main.transform);
        }
    }
}