using Core.Actors;
using UnityEngine;

namespace Actors
{
	[CreateAssetMenu(menuName = "Actors/Flying Model", fileName = "FlyingActorModel", order = 0)]
	public class FlyingActorModelImpl : ScriptableObject, IFlyingActorModel
	{
		[SerializeField]
		private float delayToAct;

		[SerializeField]
		private float moveSpeed;

		[SerializeField]
		private float takeOffSpeed;

		[SerializeField]
		private float landingSpeed;

		public float DelayToAct => delayToAct;

		public float MoveSpeed => moveSpeed;

		public float TakeOffSpeed => takeOffSpeed;

		public float LandingSpeed => landingSpeed;
	}
}