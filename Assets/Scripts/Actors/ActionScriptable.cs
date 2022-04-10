using System.Collections;
using Core.Actors;
using UnityEngine;

namespace Actors
{
	public abstract class ActionScriptable : ScriptableObject, IAction
	{
		public string Name => name;

		public abstract IEnumerator Behaviour(IActor actor, IActorModel model);
	}
}