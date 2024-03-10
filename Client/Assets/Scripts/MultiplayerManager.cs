using System.Collections.Generic;
using Colyseus;
using generated;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private ColyseusRoom<State> room;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private Dictionary<string, GameObject> activePlayers = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();

        Connect();
    }

    private bool IsMyPlayer(string sessionId) => string.Equals(sessionId, room.SessionId);

    private async void Connect()
    {
        string roomName = R.ServerCredits.RoomName;
        room = await Instance.client.JoinOrCreate<State>(roomName);

        Subscribe();
    }

    private void Subscribe()
    {
        room.OnStateChange += OnRoomStateChangeHandler;
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
            state.players.OnRemove += DestroyPlayer;
        }
    }

    private void DestroyPlayer(string key, Player player)
    {
        if (activePlayers.ContainsKey(key))
        {
            GameObject neededObject = activePlayers[key];
            player.OnChange -= neededObject.GetComponent<EnemyController>().OnChangeHandler;
            activePlayers.Remove(key);
            Destroy(neededObject);
        }
    }

    private void CreatePlayer(string key, Player player)
    {
        GameObject prefab = IsMyPlayer(key) ? playerPrefab : enemyPrefab;
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
        Quaternion rotation = Quaternion.identity;
        GameObject newPlayer = Instantiate(prefab, position, rotation);

        activePlayers.Add(key, newPlayer);

        if (!IsMyPlayer(key)) player.OnChange += newPlayer.GetComponent<EnemyController>().OnChangeHandler;
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
}