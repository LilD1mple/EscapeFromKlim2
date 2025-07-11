using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Game.Save;
using EFK2.HealthSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EFK2.Extensions
{
	public static class Extensions
	{
		public static void EnableObject(this GameObject gameObject) => gameObject.SetActive(true);

		public static void DisableObject(this GameObject gameObject) => gameObject.SetActive(false);

		public static void AddHandler(this Button button, UnityAction action) => button.onClick.AddListener(action);

		public static void RemoveHandler(this Button button, UnityAction action) => button.onClick.RemoveListener(action);

		public static void ShakeCamera(this Camera camera, float duration, float strength, float delay = 0f) => camera.DOShakePosition(duration, strength).SetDelay(delay);

		public static T PickRandomElementInCollection<T>(this T[] values) => values[Random.Range(0, values.Length)];

		public static T PickRandomElementInCollection<T>(this List<T> values) => values[Random.Range(0, values.Count)];

		public static void DamagePlayer(this Health health, EventBus eventBus, float damage)
		{
			health.TakeDamage(damage);

			eventBus.Raise(new PlayerHealthChangedSignal(health, true));
		}

		public static void PlayAudio(this AudioSource audioSource, AudioClip audioClip)
		{
			audioSource.clip = audioClip;

			audioSource.Play();
		}

		public static void CompareByTernaryOperation(this bool condition, Action trueCompare, Action falseCompare)
		{
			if (condition)
				trueCompare.Invoke();
			else
				falseCompare.Invoke();
		}

		public static KeyCode ParseSavedKey(this string key, KeyCode defaultKey)
		{
			string savedKey = SaveUtility.LoadData(key, defaultKey.ToString());

			KeyCode parsedKey = (KeyCode)Enum.Parse(typeof(KeyCode), savedKey);

			return parsedKey;
		}

		public static bool TryFindComponent<T>(this RaycastHit hit, out T component) => hit.collider.TryGetComponent(out component);

		public static float CheckDistanceBetweenTwoTransforms(this Transform transform, Transform target) => Vector3.Distance(transform.position, target.position);

		public static bool CheckMaxDistanceBetweenTwoTransforms(this Transform transform, Transform secondTransform, float maxDistance) =>
			Vector3.Distance(transform.position, secondTransform.position) < maxDistance;

		public static bool RaycastTarget(this Transform startPoint, Transform target, float maxDistance, LayerMask layer) =>
			Physics.Raycast(startPoint.position, (target.position - startPoint.position).normalized, maxDistance, layer) == false && CheckMaxDistanceBetweenTwoTransforms(startPoint, target, maxDistance);

		public static bool OverlapSphere(this Camera camera, float raduis, float distance, LayerMask layerMask, out RaycastHit result) =>
			Physics.SphereCast(camera.transform.position, raduis, camera.transform.forward, out result, distance, layerMask);
	}
}
