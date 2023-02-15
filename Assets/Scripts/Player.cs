using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{

    [SerializeField]
    Ball ball;

    NetworkVariable<bool> ready = new NetworkVariable<bool>(
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {

        DontDestroyOnLoad(gameObject);
        ready.OnValueChanged += OnReadyChanged;

        if (IsOwner)
        {
            ready.Value = false;

            GameObject.Find("Ready").GetComponent<Button>().onClick.AddListener(OnReadyClick);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnReadyClick()
    {
        ready.Value = !ready.Value;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene Loaded");
        //Instantiate<Ball>(ball);
    }

    void OnReadyChanged(bool previous, bool current)
    {
        // if (IsServer)
        // {
        //     ServerLogic();
        // }

        if (IsOwner)
        {
            print("Cambia");
            Ready_ServerRpc(ready.Value);
        }
    }

    [ClientRpc]
    void StartGame_ClientRpc()
    {
        int activeBI = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeBI + 1);
    }

    // Método B
    int readyCount = 0;
    [ServerRpc(RequireOwnership = true)]
    void Ready_ServerRpc(bool ready)
    {
        int clientCount = NetworkManager.Singleton.ConnectedClientsList.Count;

        readyCount = (ready) ? ++readyCount : --readyCount;

        print($"Jugadores listos: {readyCount}.");

        if (readyCount == clientCount)
        {
            StartGame_ClientRpc();
        }
    }

    // Método A
    void ServerLogic()
    {

        bool start = true;
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            start = start && client.PlayerObject.gameObject.GetComponent<Player>().ready.Value;
        }

        if (start)
        {
            StartGame_ClientRpc();
        }
    }


}