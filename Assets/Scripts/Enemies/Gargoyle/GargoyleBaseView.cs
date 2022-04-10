using System;
using Actors;
using Actors.Actions;
using Core.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Gargoyle
{
	public class GargoyleBaseView : MonoBehaviour, ICoroutineRunner
	{
		[Header("Setup")]
		[SerializeField]
		private FlyingActorModelImpl model;

		[Obsolete]
		[SerializeField]
		private Transform player;

		[Header("Actions")]
		[SerializeField]
		private Melee melee;

		[SerializeField]
		private FlyingActionScriptable flyingDive;

		[SerializeField]
		private FlyingActionScriptable throwFireball;

		[SerializeField]
		private FlyingActionScriptable throwFireballWithAngle;

		[Header("Debug")]
		[SerializeField]
		private bool shouldLogTransitions;

		[Header("Events")]
		[SerializeField]
		private UnityEvent onClaw;

		[SerializeField]
		private UnityEvent onTakeOff;

		[SerializeField]
		private UnityEvent onLanding;

		[SerializeField]
		private UnityEvent onDiving;

		[SerializeField]
		private UnityEvent onDived;

		[SerializeField]
		private UnityEvent onFireball;

		private FlyingActor _actor;

		private void Awake()
		{
			_actor = new FlyingActor(model,
									this,
									transform,
									player,
									shouldLogTransitions);
			GargoyleAIController controller = new GargoyleAIController(_actor,
																		melee,
																		flyingDive,
																		throwFireball,
																		throwFireballWithAngle);
			_actor.Awake();
		}
	}
}