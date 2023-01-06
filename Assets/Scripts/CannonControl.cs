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


		UnityEvent OnBulletDetonatesEvent;
		
		int remainingBullets;
		bool ableToShoot => remainingBullets > 0;

		void Start()
		{
			remainingBullets = numBullets;

			OnBulletDetonatesEvent = new UnityEvent();
			OnBulletDetonatesEvent.AddListener(OnBulletDetonates);
		}

		void Update()
		{
			if (IsOwner)
			{
				ManageRotation();
				ManageShooting();
			}
		}

		void OnBulletDetonates()
		{
			remainingBullets += 1;
		}


		void ManageShooting()
		{
			bool shootPressed = Input.GetMouseButtonDown(0);

			bool shoot = shootPressed && ableToShoot;

			if (shoot)
			{
				StartCoroutine(nameof(ShootCoroutine));
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

		IEnumerator ShootCoroutine()
		{

			remainingBullets -= 1;

			Vector3 _offset = -transform.up * spawnOffset;
			BulletControl _bullet = Instantiate<BulletControl>(bullet, transform.position + _offset, transform.rotation);
			_bullet.OnDetonatesEvent = OnBulletDetonatesEvent;

			yield return null; 


		}

		void OnDrawGizmosSelected()
		{
			Vector3 _offset = new Vector3(0, -spawnOffset, 0);
			//Gizmos.DrawSphere(transform.position + _offset, .1f);
		}

	}
}


