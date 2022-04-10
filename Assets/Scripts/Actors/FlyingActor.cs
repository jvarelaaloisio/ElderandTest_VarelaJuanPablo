using System;
using System.Collections;
using Core.Actors;
using Core.Helpers;
using IA.FSM;
using UnityEngine;

namespace Actors
{
	public class FlyingActor : IFlyingActor
	{
		private const string GroundStateKey = "Grounded";
		private const string AirStateKey = "Air";
		private readonly FiniteStateMachine<string> _stateMachine;
		private readonly ICoroutineRunner _coroutineRunner;

		/// <summary>
		/// Event fired every time the actor can Act once again.
		/// </summary>
		public event Action OnCanAct = delegate { };

		/// <summary>
		/// Event fired when the actor does an action. The string is the name of the action.
		/// </summary>
		public event Action<string> OnActing = delegate { };

		/// <summary>
		/// Event fired when the actor takes off from the ground
		/// </summary>
		public event Action OnTakeOff = delegate { };

		/// <summary>
		/// Event fired when the actor lands
		/// </summary>
		public event Action OnLanding = delegate { };

		public Transform transform { get; }

		public string CurrentState => _stateMachine.CurrentState.Name;

		public IFlyingActorModel Model { get; }

		public FlyingActor(IFlyingActorModel model,
							ICoroutineRunner coroutineRunner,
							Transform transform,
							bool shouldLogTransitions = true)
		{
			Model = model;
			_coroutineRunner = coroutineRunner;
			this.transform = transform;
			State<string> air = new State<string>(AirStateKey);
			State<string> grounded = new State<string>(GroundStateKey);

			air.AddTransition(GroundStateKey, grounded);
			grounded.AddTransition(AirStateKey, air);

			_stateMachine = FiniteStateMachine<string>
				.Build(air, transform.gameObject.name)
				.WithThisLogger(Debug.unityLogger)
				.ThatLogsTransitions(shouldLogTransitions)
				.Done();
		}

		public void RunFirstDelayBeforeAction()
			=> _coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(Model.DelayToAct));

		public void Act(IFlyingAction flyingAction, IEnumerator afterBehaviour = null)
		{
			_coroutineRunner.StartCoroutine(RunAction(flyingAction, afterBehaviour));
		}

		public void Act(IAction action, IEnumerator afterBehaviour = null)
		{
			_coroutineRunner.StartCoroutine(RunAction(action, afterBehaviour));
		}

		private IEnumerator RunAction(IFlyingAction flyingAction, IEnumerator afterBehaviour = null)
		{
			OnActing(flyingAction.Name);
			yield return flyingAction.Behaviour(this, Model);
			yield return afterBehaviour;
			yield return RaiseCanActEventAfterDelay(Model.DelayToAct);
		}

		private IEnumerator RunAction(IAction action, IEnumerator afterBehaviour = null)
		{
			OnActing(action.Name);
			yield return action.Behaviour(this, Model);
			yield return afterBehaviour;
			yield return RaiseCanActEventAfterDelay(Model.DelayToAct);
		}

		public void TakeOff(IEnumerator takeOffBehaviour)
			=> _coroutineRunner.StartCoroutine(RunStateChangeBehaviour(takeOffBehaviour, OnTakeOff, AirStateKey));

		public void Land(IEnumerator landBehaviour)
			=> _coroutineRunner.StartCoroutine(RunStateChangeBehaviour(landBehaviour, OnLanding, GroundStateKey));

		public bool IsGrounded() => CurrentState == GroundStateKey;

		private IEnumerator RaiseCanActEventAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			OnCanAct();
		}

		private IEnumerator RunStateChangeBehaviour(IEnumerator behaviour,
													Action onFinish,
													string stateMachineTransitionKey)
		{
			yield return behaviour;
			onFinish();
			_stateMachine.TransitionTo(stateMachineTransitionKey);
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(Model.DelayToAct));
		}
	}
}