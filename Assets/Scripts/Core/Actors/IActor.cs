using System;
using System.Collections;
using UnityEngine;

namespace Core.Actors
{
	public interface IActor
	{
		event Action OnCanAct;
		event Action<string> OnActing;
		Transform transform { get; }
		string CurrentState { get; }
		void RunFirstDelayBeforeAction();
		void Act(IAction action, IEnumerator afterBehaviour = null);
	}
}