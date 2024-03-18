using System;
using System.Collections;
using System.Collections.Generic;
using Gun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private float mouseSensetivity = 2f;
    [SerializeField] private PlayerGun gun;

    private float restartDelay = 3f;
    private bool hold = false;

    private void Update()
    {
        if (hold) return;

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

    private void SendMoveForRestart(RestartInfo restartInfo)
    {
        string message = R.ToServerEvents.Move;
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", restartInfo.x },
            { "pY", 0 },
            { "pZ", restartInfo.z },
            { "vX", 0 },
            { "vY", 0 },
            { "vZ", 0 },
            { "rX", 0 },
            { "rY", 0 },
        };

        MultiplayerManager.Instance.SendMessage(message, data);
    }

    public void OnPlayerRestartHandler(string json)
    {
        IEnumerator waiting()
        {
            hold = true;
            yield return new WaitForSeconds(restartDelay);
            hold = false;
        }

        StartCoroutine(waiting());

        RestartInfo restartInfo = JsonUtility.FromJson<RestartInfo>(json);
        playerCharacter.transform.position = new Vector3(restartInfo.x, 0, restartInfo.z);
        playerCharacter.SetInput(0, 0, 0);
        SendMoveForRestart(restartInfo);
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

[Serializable]
public struct RestartInfo
{
    public float x;
    public float z;
}