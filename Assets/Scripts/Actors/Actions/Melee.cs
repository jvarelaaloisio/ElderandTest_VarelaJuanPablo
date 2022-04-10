using System.Collections;
using Core.Actors;
using Core.Helpers;
using UnityEngine;

namespace Actors.Actions
{
	[CreateAssetMenu(menuName = "Actors/Actions/Melee", fileName = "Melee", order = 0)]
	public class Melee : ActionScriptable
	{
		[Tooltip("Damage dealt to each target")]
		[SerializeField]
		private int damage;

		[Tooltip("The maximum distance to which the melee can hit")]
		[SerializeField]
		private float range;

		[Tooltip("Maximum quantity of targets")]
		[SerializeField]
		private int maximumTargets = 5;

		[Tooltip("Layers where the possible targets are")]
		[SerializeField]
		private LayerMask targetLayer;

		[Tooltip("Delay before the damage logic")]
		[SerializeField]
		private float delay;

		[Tooltip("time before yielding control once the damage has been dealt")]
		[SerializeField]
		private float duration;

		[Tooltip("Turn this flag on to see this action log when it runs and give data about it's behaviour")]
		[SerializeField]
		private bool shouldLogBehaviour;

		public override IEnumerator Behaviour(IActor actor, IActorModel model)
		{
			yield return new WaitForSeconds(delay);
			Collider2D[] targets = new Collider2D[maximumTargets];
			string logData = $"{actor.transform.name.Bold()}: I have used the action {name.Bold().Colored("green")}";
			if (CanHit(actor, targets))
				foreach (Collider2D collider in targets)
				{
					if (collider == null
						|| collider.transform == actor.transform
						|| !collider.TryGetComponent(out IDamageable damageable))
						continue;
					CollectLogData(collider, damage, ref logData);

					damageable.TakeDamage(damage);
				}
			else
				logData += "\nThere were no victims.";
					
			if (shouldLogBehaviour)
				Debug.Log(logData, actor.transform);

			yield return new WaitForSeconds(duration);
		}

		public bool CanHit(IActor actor, Collider2D[] targets)
		{
			return Physics2D.OverlapCircleNonAlloc(actor.transform.position,
													range,
													targets,
													targetLayer) > 1;
		}

		private static void CollectLogData(Collider2D collider, int damage, ref string currentLogData)
			=> currentLogData += $"\n{damage.Colored("red")} damage points have been dealt" +
								$" to the damageable: {collider.name.Colored("yellow")}";
	}
}