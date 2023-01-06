using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;


namespace Tank
{
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

			StartCoroutine(nameof(DetonateCoroutine));
		}

		IEnumerator DetonateCoroutine()
		{

			if (OnDetonateEvent == null) throw new System.Exception("BulletControl: OnDetonateEvent is null (DetonateCoroutine)");

			OnDetonateEvent.Invoke();

			yield return null;

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
			transform.position += -transform.up * velocity * Time.deltaTime;
		}
	}
}