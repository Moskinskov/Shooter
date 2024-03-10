using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter enemyCharacter;
    private readonly List<float> allTimeIntervals = new List<float>() { 0f, 0f, 0f, 0f, 0f };
    private float lastReceivedTime = 0f;

    private float AverageInterval
    {
        get
        {
            float summ = 0f;
            foreach (float interval in allTimeIntervals) summ += interval;
            return summ / allTimeIntervals.Count;
        }
    }

    private void SaveReceivedTime()
    {
        float interval = Time.time - lastReceivedTime;
        lastReceivedTime = Time.time;
        allTimeIntervals.Add(interval);
        allTimeIntervals.RemoveAt(0);
    }


    public void OnChangeHandler(List<DataChange> changes)
    {
        SaveReceivedTime();

        Vector3 position = transform.position;
        Vector3 velocity = Vector3.zero;

        foreach (DataChange dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                default:
                    break;
            }
        }

        enemyCharacter.SetMovement(position, velocity, AverageInterval);
    }
}