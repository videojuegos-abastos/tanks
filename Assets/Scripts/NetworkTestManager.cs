using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class NetworkTestManager : NetworkBehaviour
{

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void StartClient()
    {
        GameObject c = GameObject.Find("Client");
        string ip = c.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
        string password = c.transform.GetChild(1).GetChild(2).GetComponent<Text>().text;

        if (ip != null && ip != string.Empty)
        {
            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().ConnectionData.Address = ip;
        }

        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(password);

        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Soy host");
        NetworkManager.Singleton.StartHost();
    }
}
