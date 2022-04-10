using System.Collections;
using Core.Actors;
using UnityEngine;

namespace Actors
{
	public abstract class ActionScriptable : ScriptableObject, IAction
	{
		[Tooltip("The seconds before the logic is executed" +
				"\nSet to 0 to have no delay")]
		[SerializeField]
		private float delay;

		[Tooltip("The seconds before yielding control once the damage has been dealt" +
				"\nSet to 0 to yield control as soon as the logic is done")]
		[SerializeField]
		private float duration;

		public string Name => name;

		public IEnumerator Behaviour(IActor actor, IActorModel model)
		{
			yield return new WaitForSeconds(delay);
			yield return BehaviourInternal(actor, model);
			yield return new WaitForSeconds(duration);
		}

		protected abstract IEnumerator BehaviourInternal(IActor actor, IActorModel model);
	}
}