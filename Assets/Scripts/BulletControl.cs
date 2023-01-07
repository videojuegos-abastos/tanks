using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;


namespace Tank
{
	// Toda la lógica se ejecuta en el servidor
	public class BulletControl : NetworkBehaviour
	{

		[SerializeField]
		float velocity;

		[SerializeField]
		int countDownTime;

		public UnityEvent OnDetonateEvent;


		IEnumerator CountDownCoroutine()
		{

			yield return new WaitForSeconds(countDownTime);

			Detonate();
		}

		void Detonate()
		{

			if (OnDetonateEvent == null) throw new System.Exception("BulletControl: OnDetonateEvent is null (DetonateCoroutine)");

			OnDetonateEvent.Invoke();

			Destroy(gameObject);
			//gameObject.SetActive(false);
		}


		void OnEnable()
		{
			StartCoroutine(nameof(CountDownCoroutine));
		}

		void OnDisable()
		{
			StopCoroutine(nameof(CountDownCoroutine));

		}

		void Update()
		{
			if (!IsServer) return;
			transform.position += -transform.up * velocity * Time.deltaTime;
		}
	}
}