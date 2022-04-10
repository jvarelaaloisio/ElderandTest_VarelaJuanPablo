using System;
using Actors;
using Actors.Actions;
using Core.Helpers;
using Enemies.Gargoyle.Actions;
using Paths;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Gargoyle
{
	public class GargoyleView : MonoBehaviour, ICoroutineRunner
	{
		[Header("Setup")]
		[SerializeField]
		private FlyingActorModelImpl model;

		[SerializeField]
		private PathTraveler pathTraveler;

		[SerializeField]
		private Animator animator;
		
		[Header("Actions")]
		[SerializeField]
		private Melee melee;

		[SerializeField]
		private FlyingActionScriptable flyingDive;

		[Tooltip("Spawn Position shown with a red icon")]
		[SerializeField]
		private ThrowFireball throwHorizontalFireball;

		[Tooltip("Spawn Position shown with a yellow icon")]
		[SerializeField]
		private ThrowFireball throwDiagonalFireball;

		[Header("Debug")]
		[SerializeField]
		private bool shouldLogTransitions;

		[SerializeField]
		private bool shouldLogTreeDecisions;

		[Header("Events")]
		[SerializeField]
		private UnityEvent onTakeOff;

		[SerializeField]
		private UnityEvent onLanding;

		[SerializeField]
		private UnityEvent onClaw;

		[SerializeField]
		private UnityEvent onFireball;

		[SerializeField]
		private UnityEvent onDiving;
		
		private FlyingActor _actor;


		private void OnValidate()
		{
			if (!pathTraveler)
				TryGetComponent(out pathTraveler);
		}

		private void Awake()
		{
			_actor = new FlyingActor(model,
									this,
									transform,
									shouldLogTransitions);
			_actor.OnLanding += onLanding.Invoke;
			_actor.OnTakeOff += onTakeOff.Invoke;
			_actor.OnActing += ReactToAction;
			GargoyleAIController controller = new GargoyleAIController(_actor,
																		pathTraveler,
																		melee,
																		flyingDive,
																		throwHorizontalFireball,
																		throwDiagonalFireball,
																		shouldLogTreeDecisions);
			_actor.RunFirstDelayBeforeAction();
		}

		private void Update()
		{
			transform.right = pathTraveler.IsGoingRight ? Vector3.right : Vector3.left;
		}

		private void ReactToAction(string action)
		{
			if (action == melee.Name)
				onClaw.Invoke();
			else if (action == throwHorizontalFireball.Name
					|| action == throwDiagonalFireball.Name)
				onFireball.Invoke();
			else if (action == flyingDive.Name) onDiving.Invoke();
		}

		private void OnDrawGizmos()
		{
			if (melee)
			{
				Gizmos.color = new Color(.0f, .75f, .25f, .5f);
				Gizmos.DrawWireSphere(transform.position, melee.Range);
			}

			if (throwHorizontalFireball)
			{
				Gizmos.DrawIcon(transform.position + transform.TransformDirection(throwHorizontalFireball.SpawnOffset),
								"forward@2x",
								false,
								Color.red);
			}

			if (throwDiagonalFireball)
			{
				Vector3 spawnPosition =
					transform.position + transform.TransformDirection(throwDiagonalFireball.SpawnOffset);
				Gizmos.DrawIcon(spawnPosition,
								"curvekeyframeselected",
								false,
								Color.yellow);
				Vector3 throwDirection = Quaternion.Euler(0, 0, throwDiagonalFireball.Angle) * transform.right;
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(spawnPosition, throwDirection / 2);
			}
		}
	}
}