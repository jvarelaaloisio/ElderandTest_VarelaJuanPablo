using System;
using UnityEngine;

namespace Core.Actors
{
	public interface IActor
	{
		event Action OnCanAct;
		event Action<string> OnActing;
		Transform transform { get; }
		string CurrentState { get; }
		void Awake();
		void Act(IAction action);
	}
}