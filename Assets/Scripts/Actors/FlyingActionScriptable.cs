using System.Collections;
using Core.Actors;
using UnityEngine;

namespace Actors
{
	public abstract class FlyingActionScriptable : ScriptableObject, IFlyingAction
	{
		public string Name => name;

		public abstract IEnumerator Behaviour(IFlyingActor flyingActor, IFlyingActorModel model);
	}
}