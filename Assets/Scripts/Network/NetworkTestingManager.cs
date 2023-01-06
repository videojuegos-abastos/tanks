using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTestingManager : MonoBehaviour
{
	void Start()
	{
		if (Application.isEditor)
		{
			NetworkManager.Singleton.StartHost();
		}
		else
		{
			NetworkManager.Singleton.StartClient();
		}
	}

}
