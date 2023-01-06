using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace Tank
{

	public class TankControl : NetworkBehaviour
	{

		[SerializeField]
		float velocity;

		[SerializeField]
		[Tooltip("Degrees per second")]
		float rotationVelocity;

		void Start()
		{
			
		}

		void Update()
		{

			if (IsOwner)
			{	
				ManageRotation();
				ManageMovement();
			}
		}


		void ManageMovement()
		{
			transform.position += -transform.up * velocity * Time.deltaTime;
		}

		void ManageRotation()
		{
			Vector3 _mousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 _direction = (Vector2) (_mousePosition - transform.position);

			float _angleRad = Mathf.Atan2(_direction.y, _direction.x);
			float _angleDeg = Mathf.Rad2Deg * _angleRad;
			
			Quaternion _targetRotation = Quaternion.Euler(0, 0, _angleDeg + 90);
			Quaternion _rotation = Quaternion.RotateTowards(transform.localRotation, _targetRotation, rotationVelocity * Time.deltaTime);
			transform.localRotation = _rotation;
		}



	}
}