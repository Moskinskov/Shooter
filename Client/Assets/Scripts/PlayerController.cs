using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        playerCharacter.SetInput(h, v);

        SendMove();
    }

    private void SendMove()
    {
        string message = R.ToServerEvents.Move;
        playerCharacter.GetMoveInfo(out Vector3 position, out Vector3 velocity);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", velocity.x },
            { "vY", velocity.y },
            { "vZ", velocity.z },
        };

        MultiplayerManager.Instance.SendMessage(message, data);
    }
}