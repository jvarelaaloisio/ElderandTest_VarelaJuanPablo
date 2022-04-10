using System;
using Core.Actors;
using UnityEngine;

namespace Enemies
{
	[CreateAssetMenu(menuName = "Actors/Flying Model", fileName = "FlyingActorModel", order = 0)]
	public class FlyingActorModelImpl : ScriptableObject, IFlyingActorModel
	{
		[Obsolete]
		[SerializeField]
		private float meleeDistance;

		[SerializeField]
		private float delayToAct;

		[SerializeField]
		private float moveSpeed;

		[SerializeField]
		private float takeOffSpeed;

		[SerializeField]
		private float ladingSpeed;

		[SerializeField]
		private float flyingDiveSpeed;

		[SerializeField]
		private float fireballSpeed;

		[SerializeField]
		private float fireballDamage;

		[SerializeField]
		private float clawDamage;

		[SerializeField]
		private float flyingDiveDamage;

		[Obsolete]
		public float MeleeDistance => meleeDistance;

		public float DelayToAct => delayToAct;

		public float MoveSpeed => moveSpeed;

		public float TakeOffSpeed => takeOffSpeed;

		public float LadingSpeed => ladingSpeed;

		public float FlyingDiveSpeed => flyingDiveSpeed;

		public float FireballSpeed => fireballSpeed;

		public float FireballDamage => fireballDamage;

		public float ClawDamage => clawDamage;

		public float FlyingDiveDamage => flyingDiveDamage;
	}
}