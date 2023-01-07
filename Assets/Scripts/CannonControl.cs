using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.Networking;

namespace Tank
{
	public class CannonControl : NetworkBehaviour
	{

		[SerializeField]
		BulletControl bullet;


		[SerializeField]
		float spawnOffset;

		[SerializeField]
		int numBullets;


		UnityEvent OnBulletDetonateEvent;
		
		NetworkVariable<int> remainingBullets = new NetworkVariable<int>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
		bool ableToShoot => remainingBullets.Value > 0;

		void Start()
		{

			remainingBullets.Value = numBullets;

			OnBulletDetonateEvent = new UnityEvent();
			OnBulletDetonateEvent.AddListener(OnBulletDetonate);
		}

		void Update()
		{
			if (IsOwner)
			{
				ManageRotation();
				ManageShooting();
			}
		}

		void OnBulletDetonate()
		{
			remainingBullets.Value += 1;
		}


		void ManageShooting()
		{
			bool shootPressed = Input.GetMouseButtonDown(0);

			bool shoot = shootPressed && ableToShoot;

			if (shoot)
			{
				Shoot_ServerRpc();
			}

		}

		void ManageRotation()
		{
			Vector3 _mousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Vector2 _direction = (Vector2) (_mousePosition - transform.position);

			float _angleRad = Mathf.Atan2(_direction.y, _direction.x);

			float _angleDeg = Mathf.Rad2Deg * _angleRad;
			this.transform.rotation = Quaternion.Euler(0, 0, _angleDeg + 90);
		}


		[ServerRpc(RequireOwnership = true)]
		void Shoot_ServerRpc(ServerRpcParams rpcParams = default)
		{

			remainingBullets.Value -= 1;

			Vector3 _offset = -transform.up * spawnOffset;
			BulletControl _bullet = Instantiate<BulletControl>(bullet, transform.position + _offset, transform.rotation);
			_bullet.OnDetonateEvent = OnBulletDetonateEvent;


			//_bullet.GetComponent<NetworkObject>().Spawn();
			_bullet.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
		}

		void OnDrawGizmosSelected()
		{
			Vector3 _offset = new Vector3(0, -spawnOffset, 0);
			//Gizmos.DrawSphere(transform.position + _offset, .1f);
		}

	}
}


