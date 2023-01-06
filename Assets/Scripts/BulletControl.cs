using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Tank
{
	public class BulletControl : MonoBehaviour
	{

		[SerializeField]
		float velocity;

		[SerializeField]
		int countDownTime;

		public UnityEvent OnDetonatesEvent;


		IEnumerator CountDownCoroutine()
		{

			yield return new WaitForSeconds(countDownTime);

			StartCoroutine(nameof(DetonateCoroutine));
		}

		IEnumerator DetonateCoroutine()
		{

			if (OnDetonatesEvent == null) throw new System.Exception("BulletControl: OnDetonatesEvent is null (DetonateCoroutine)");

			OnDetonatesEvent.Invoke();

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