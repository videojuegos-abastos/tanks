using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{

    public static Player owner;

    public NetworkVariable<bool> ready {get; private set;} = new NetworkVariable<bool>(
        value: false,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);

    PlayerManager playerManager;
    public override void OnNetworkSpawn()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.AddPlayer(this);

        DontDestroyOnLoad(gameObject);

        if (IsOwner)
        {
            owner = this;

            // Me suscribo al onClick del botón ready para cambiar el valor de ready
            // y al evento sceneLoaded para enterarme de cuando hemos cambiado de escena
            GameObject.Find("Ready").GetComponent<Button>().onClick.AddListener(OnReadyClick);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        if (IsServer)
        {
            ready.OnValueChanged += (bool _, bool _) => CheckAllReady();
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


    // Comprobamos si todos los clientes están en "Ready"
    void CheckAllReady()
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

    // Cambiamos de escena en todos los clientes
    [ClientRpc]
    void StartGame_ClientRpc()
    {
        // Cambio a la siguiente escena
        int activeBI = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeBI + 1);
    }


}