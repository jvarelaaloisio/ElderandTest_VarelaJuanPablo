using System.Collections;
using Actors;
using Core.Actors;
using Paths;
using UnityEngine;

namespace Enemies.Gargoyle.Actions
{
	[CreateAssetMenu(menuName = "Actors/Actions/Flying Dive", fileName = "FlyingDive", order = 2)]
	public class FlyingDive : FlyingActionScriptable
	{
		[SerializeField]
		private float speed;

		protected override IEnumerator BehaviourInternal(IFlyingActor flyingActor, IFlyingActorModel model)
		{
			if (flyingActor.transform.TryGetComponent(out PathTraveler traveler))
				yield return traveler.Dive(1 / speed);
		}
	}
}