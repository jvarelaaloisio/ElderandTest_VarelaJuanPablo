using System.Collections;
using Actors;
using Core.Actors;
using UnityEngine;

namespace Enemies.Gargoyle.Actions
{
	[CreateAssetMenu(menuName = "Actors/Actions/Throw Fireball", fileName = "ThrowFireball", order = 1)]
	public class ThrowFireball : ActionScriptable
	{
		[Header("Game Design")]
		[SerializeField]
		private float angle;

		[Header("Setup")]
		[SerializeField]
		private GameObject fireballPrefab;

		[Tooltip("The offset from the actor's position, where the fireball will be instantiated")]
		[SerializeField]
		private Vector3 spawnOffset;

		public float Angle => angle;

		public GameObject FireballPrefab => fireballPrefab;

		public Vector3 SpawnOffset => spawnOffset;

		protected override IEnumerator BehaviourInternal(IActor actor, IActorModel model)
		{
			GameObject.Instantiate(FireballPrefab,
									actor.transform.position + actor.transform.TransformDirection(SpawnOffset),
									actor.transform.rotation * Quaternion.Euler(0, 0, Angle));
			yield break;
		}
	}
}