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
		private const string GroundedStateKey = "Grounded";
		private const string FlyingStateKey = "Flying";
		private readonly FiniteStateMachine<string> _stateMachine;
		private readonly IFlyingActorModel _model;
		private readonly ICoroutineRunner _coroutineRunner;

		public event Action OnCanAct = delegate { };
		public event Action<string> OnActing = delegate { };

		public event Action OnTakeOff = delegate { };
		public event Action OnLanding = delegate { };
		public Transform transform { get; }
		public Transform Target { get; }

		public string CurrentState => _stateMachine.CurrentState.Name;

		public FlyingActor(IFlyingActorModel model,
							ICoroutineRunner coroutineRunner,
							Transform transform,
							Transform target,
							bool shouldLogTransitions = true)
		{
			_model = model;
			_coroutineRunner = coroutineRunner;
			this.transform = transform;
			Target = target;
			State<string> flying = new State<string>(FlyingStateKey);
			State<string> grounded = new State<string>(GroundedStateKey);

			flying.AddTransition(GroundedStateKey, grounded);
			grounded.AddTransition(FlyingStateKey, flying);

			_stateMachine = FiniteStateMachine<string>
				.Build(flying, transform.gameObject.name)
				.WithThisLogger(Debug.unityLogger)
				.ThatLogsTransitions(shouldLogTransitions)
				.Done();
		}

		//TODO:Change Name
		public void Awake()
			=> _coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));

		public void Act(IFlyingAction flyingAction)
		{
			_coroutineRunner.StartCoroutine(RunAction(flyingAction));
		}

		//TODO:Move To base Actor class
		public void Act(IAction action)
		{
			_coroutineRunner.StartCoroutine(RunAction(action));
		}

		private IEnumerator RunAction(IFlyingAction flyingAction)
		{
			OnActing(flyingAction.Name);
			yield return flyingAction.Behaviour(this, _model);
			yield return RaiseCanActEventAfterDelay(_model.DelayToAct);
		}

		//TODO:Move To base Actor class
		private IEnumerator RunAction(IAction action)
		{
			OnActing(action.Name);
			yield return action.Behaviour(this, _model);
			yield return RaiseCanActEventAfterDelay(_model.DelayToAct);
		}

		public void TakeOff()
		{
			OnTakeOff();
			//TODO:Impplement
			_stateMachine.TransitionTo(FlyingStateKey);
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		public void Land()
		{
			OnLanding();
			//TODO:Impplement
			_stateMachine.TransitionTo(GroundedStateKey);
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		public void Melee()
		{
			// OnMelee();
			//TODO:Implement
			//Target.damageable.takeDamage
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		public void FlyingDive()
		{
			// OnDiving();
			// OnDived();
			//TODO:Implement
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		public void ThrowFireBall()
		{
			// OnFireball();
			//TODO:Implement
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		public void ThrowFireBallWithAngle()
		{
			//TODO:Implement
			_coroutineRunner.StartCoroutine(RaiseCanActEventAfterDelay(_model.DelayToAct));
			throw new NotImplementedException();
		}

		[Obsolete]
		public bool IsTargetClose()
			=> Vector3.Distance(Target.position, transform.position) <= _model.MeleeDistance;

		public bool IsGrounded() => CurrentState == GroundedStateKey;

		private IEnumerator RaiseCanActEventAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			OnCanAct();
		}
	}
}