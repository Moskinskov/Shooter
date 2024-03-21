using UnityEngine;

namespace DefaultNamespace
{
    public class SpawnPoints : MonoBehaviour
    {
        [SerializeField] private Transform[] points;

        public int Length => points.Length;

        public void GetPoint(int index, out Vector3 position, out Vector3 rotation)
        {
            if (index < 0 || index >= Length)
            {
                position = Vector3.zero;
                rotation = Vector3.zero;
            }
            else
            {
                position = points[index].position;
                rotation = points[index].rotation.eulerAngles;
            }
        }
    }
}