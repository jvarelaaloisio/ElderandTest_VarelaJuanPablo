using System.Collections;
using Core.Actors;
using Core.Helpers;
using UnityEngine;

namespace Actors.Actions
{
	[CreateAssetMenu(menuName = "Actors/Actions/Melee", fileName = "Melee", order = 0)]
	public class Melee : ActionScriptable
	{
		[Header("Game Design")]
		[Tooltip("Damage dealt to each target")]
		[SerializeField]
		private int damage;

		[Tooltip("The maximum distance to which the melee can hit")]
		[SerializeField]
		private float range;

		[Header("Setup")]
		[Tooltip("Maximum quantity of targets")]
		[SerializeField]
		private int maximumTargets = 5;

		[Tooltip("Layers where the possible targets are")]
		[SerializeField]
		private LayerMask targetLayer;

		[Header("Debug")]
		[Tooltip("Turn this flag on to see this action log when it runs and give data about it's behaviour")]
		[SerializeField]
		private bool shouldLogBehaviour;

		public int Damage => damage;
		public float Range => range;
		public int MaximumTargets => maximumTargets;
		public LayerMask TargetLayer => targetLayer;

		protected override IEnumerator BehaviourInternal(IActor actor, IActorModel model)
		{
			Collider2D[] targets = new Collider2D[MaximumTargets];
			string logData = $"{actor.transform.name.Bold()}: I have used the action {name.Bold().Colored("#00AA55")}";
			if (CanHit(actor, targets))
				foreach (Collider2D collider in targets)
				{
					if (collider == null
						|| collider.transform == actor.transform
						|| !collider.TryGetComponent(out IDamageable damageable))
						continue;
					CollectLogData(collider, Damage, ref logData);

					damageable.TakeDamage(Damage);
				}
			else
				logData += "\nThere were no victims.";
					
			if (shouldLogBehaviour)
				Debug.Log(logData, actor.transform);
			yield break;
		}

		public bool CanHit(IActor actor, Collider2D[] targets)
		{
			return Physics2D.OverlapCircleNonAlloc(actor.transform.position,
													Range,
													targets,
													TargetLayer) > 1;
		}

		private static void CollectLogData(Collider2D collider, int damage, ref string currentLogData)
			=> currentLogData += $"\n{damage.Colored("#FF3333").Bold()} damage points have been dealt" +
								$" to the damageable: {collider.name.Colored("yellow")}";
	}
}