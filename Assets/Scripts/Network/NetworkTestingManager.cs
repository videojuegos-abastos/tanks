using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTestingManager : NetworkBehaviour
{
	void Start()
	{
		StartNetwork();
	}

	public override void OnDestroy()
	{
		StartNetwork();
	}

	void StartNetwork()
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
