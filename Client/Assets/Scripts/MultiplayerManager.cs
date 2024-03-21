using System.Collections.Generic;
using Colyseus;
using DefaultNamespace;
using generated;
using UI;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private ColyseusRoom<State> room;
    [field: SerializeField] public SpawnPoints spawnPoints { get; private set; }
    [SerializeField] private PlayerCharacter playerPrefab;
    [SerializeField] private EnemyController enemyPrefab;
    [field: SerializeField] public LossCounterUi lossCounterUi { get; set; }
    private Dictionary<string, EnemyController> enemies = new Dictionary<string, EnemyController>();

    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();

        Connect();
    }

    private bool IsMyPlayer(string sessionId) => string.Equals(sessionId, room.SessionId);

    public string GetClientKey => room.SessionId;

    private async void Connect()
    {
        spawnPoints.GetPoint(Random.Range(0, spawnPoints.Length), out Vector3 spawnPosition, out Vector3 spawnRotation);

        Dictionary<string, object> joinParams = new Dictionary<string, object>()
        {
            { "speed", playerPrefab.Speed },
            { "hp", playerPrefab.MaxHealth },
            { "points", spawnPoints.Length },
            { "pX", spawnPosition.x },
            { "pY", spawnPosition.y },
            { "pZ", spawnPosition.z },
            { "rY", spawnRotation.y },
        };

        string roomName = R.ServerCredits.RoomName;
        room = await Instance.client.JoinOrCreate<State>(roomName, joinParams);

        Subscribe();
    }

    private void Subscribe()
    {
        room.OnStateChange += OnRoomStateChangeHandler;
        room.OnMessage<string>(R.FromServerEvents.Shoot, OnShootHandler);
    }

    private void OnShootHandler(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        string key = shootInfo.key;
        if (enemies.TryGetValue(key, out EnemyController enemy)) enemy.Shoot(shootInfo);
    }

    private void Unsubscribe()
    {
        room.OnStateChange -= OnRoomStateChangeHandler;
    }

    private void OnRoomStateChangeHandler(State state, bool isFirstState)
    {
        if (isFirstState)
        {
            state.players.ForEach(CreatePlayer);
            state.players.OnAdd += CreatePlayer;
            state.players.OnRemove += RemovePlayer;
            room.OnStateChange -= OnRoomStateChangeHandler;
        }
    }

    private void RemovePlayer(string key, Player player)
    {
        if (enemies.ContainsKey(key)) enemies.Remove(key);
    }

    private void CreatePlayer(string key, Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
        Quaternion rotation = Quaternion.Euler(0, player.rY, 0);

        if (IsMyPlayer(key))
        {
            string message = R.FromServerEvents.Restart;
            PlayerCharacter newPlayer = Instantiate(playerPrefab, position, rotation);
            player.OnChange += newPlayer.OnChange;
            room.OnMessage<int>(message, newPlayer.GetComponent<PlayerController>().OnPlayerRestartHandler);
        }
        else
        {
            EnemyController newPlayer = Instantiate(enemyPrefab, position, rotation);
            newPlayer.Init(key, player);
            enemies.Add(key, newPlayer);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (room is not null)
        {
            Unsubscribe();
            room.Leave();
        }
    }

    public void SendMessage(string message, Dictionary<string, object> data)
    {
        room.Send(message, data);
    }

    public void SendMessage(string message, string data)
    {
        room.Send(message, data);
    }
}