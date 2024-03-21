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
    private bool hideCursor = false;

    private void Start()
    {
        hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hideCursor = !hideCursor;
            Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (hold) return;

        if (hideCursor == false) return;

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

    private void SendMoveForRestart(Vector3 position, Vector3 rotation)
    {
        string message = R.ToServerEvents.Move;
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", 0 },
            { "vY", 0 },
            { "vZ", 0 },
            { "rX", 0 },
            { "rY", rotation.y },
        };

        MultiplayerManager.Instance.SendMessage(message, data);
    }

    public void OnPlayerRestartHandler(int spawnIndex)
    {
        IEnumerator waiting()
        {
            hold = true;
            yield return new WaitForSeconds(restartDelay);
            hold = false;
        }

        StartCoroutine(waiting());

        MultiplayerManager.Instance.spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);
        playerCharacter.transform.position = position;
        rotation.x = 0;
        rotation.z = 0;
        playerCharacter.transform.eulerAngles = rotation;
        playerCharacter.SetInput(0, 0, 0);
        SendMoveForRestart(position, rotation);
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