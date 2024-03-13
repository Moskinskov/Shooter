using System.Collections.Generic;
using Colyseus.Schema;
using generated;
using Gun;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter enemyCharacter;
    [SerializeField] private EnemyGun gun;
    private readonly List<float> allTimeIntervals = new List<float>() { 0f, 0f, 0f, 0f, 0f };
    private float lastReceivedTime = 0f;
    private Player player;

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


    public void Init(Player player)
    {
        this.player = player;
        enemyCharacter.SetSpeed(player.speed);
        this.player.OnChange += OnPlayerChangeHandler;
        this.player.OnRemove += OnPlayerRemoveHandler;
    }

    public void Shoot(in ShootInfo shootInfo)
    {
        Vector3 position = new Vector3(shootInfo.pX, shootInfo.pY, shootInfo.pZ);
        Vector3 velocity = new Vector3(shootInfo.dX, shootInfo.dY, shootInfo.dZ);
        gun.Shoot(position, velocity);
    }

    private void OnPlayerRemoveHandler()
    {
        if (player is not null)
        {
            player.OnChange -= OnPlayerChangeHandler;
            player.OnRemove -= OnPlayerRemoveHandler;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnPlayerRemoveHandler();
    }

    public void OnPlayerChangeHandler(List<DataChange> changes)
    {
        SaveReceivedTime();

        Vector3 position = enemyCharacter.TargetPosition;
        Vector3 velocity = enemyCharacter.Velocity;

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
                case "rX":
                    enemyCharacter.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    enemyCharacter.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    break;
            }
        }

        enemyCharacter.SetMovement(position, velocity, AverageInterval);
    }
}