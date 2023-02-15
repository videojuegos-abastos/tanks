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
        if (IsServer)
        {
            ServerLogic();
        }
    }

    [ClientRpc]
    void StartGame_ClientRpc()
    {
        int activeBI = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeBI + 1);
    }

    [ServerRpc]
    void Ready_ServerRpc()
    {
    }

    void Update()
    {
    }

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