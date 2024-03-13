using System;
using System.Collections.Generic;
using Gun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private float mouseSensetivity = 2f;
    [SerializeField] private PlayerGun gun;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);

        bool space = Input.GetKey(KeyCode.Space);

        playerCharacter.SetInput(h, v, mouseX * mouseSensetivity);
        playerCharacter.RotateX(-mouseY * mouseSensetivity);

        if (space) playerCharacter.Jump();
        if (isShoot && gun.TryShoot(out ShootInfo shootInfo))
        {
            shootInfo.key = MultiplayerManager.Instance.GetClientKey;
            SendShoot(shootInfo);
        }

        SendMove();
    }

    private void SendShoot(in ShootInfo shootInfo)
    {
        string message = R.ToServerEvents.Shoot;
        string data = JsonUtility.ToJson(shootInfo);
        MultiplayerManager.Instance.SendMessage(message, data);
    }

    private void SendMove()
    {
        string message = R.ToServerEvents.Move;
        playerCharacter.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", velocity.x },
            { "vY", velocity.y },
            { "vZ", velocity.z },
            { "rX", rotateX },
            { "rY", rotateY },
        };

        MultiplayerManager.Instance.SendMessage(message, data);
    }
}

[Serializable]
public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}