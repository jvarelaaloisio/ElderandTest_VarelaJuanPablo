using System;
using System.Collections.Generic;
using Actors;
using Actors.Actions;
using Core.Actors;
using IA.DecisionTree;
using UnityEngine;

namespace Enemies.Gargoyle
{
	public class GargoyleAIController
	{
		private const string FlyKey = "Fly";
		private const string LandKey = "Land";
		private const string MeleeKey = "Claw";
		private const string FlyingDiveKey = "FlyingDive";
		private const string HorizontalFireball = "HorizontalFireball";
		private const string DiagonalFireball = "DiagonalFireball";
		private readonly FlyingActor _actor;
		private readonly Melee _meleeAction;
		private readonly IFlyingAction _flyingDiveAction;
		private readonly IFlyingAction _throwFireballAction;
		private readonly IFlyingAction _throwFireballWithAngleAction;
		private Tree<string> _tree;
		private readonly Dictionary<string, Action> _actionsWithBehaviours;
		private string _lastTreeAnswer;

		public GargoyleAIController(FlyingActor actor,
									Melee meleeAction,
									IFlyingAction flyingDiveAction,
									IFlyingAction throwFireballAction,
									IFlyingAction throwFireballWithAngleAction)
		{
			_actor = actor;
			_meleeAction = meleeAction;
			_flyingDiveAction = flyingDiveAction;
			_throwFireballAction = throwFireballAction;
			_throwFireballWithAngleAction = throwFireballWithAngleAction;
			//------ Setup Tree  ------
			TreeAction<string> fly = new TreeAction<string>(FlyKey);
			TreeAction<string> land = new TreeAction<string>(LandKey);
			TreeAction<string> claw = new TreeAction<string>(MeleeKey);
			TreeAction<string> flyingDive = new TreeAction<string>(FlyingDiveKey);
			TreeAction<string> horizontalFireball = new TreeAction<string>(HorizontalFireball);
			TreeAction<string> diagonalFireball = new TreeAction<string>(DiagonalFireball);

			Dictionary<INode, int> groundOutcomesWithChance = new Dictionary<INode, int>()
															{
																{horizontalFireball, 80},
																{fly, 20}
															};
			TreeRoulette groundedRoulette = new TreeRoulette(groundOutcomesWithChance);

			TreeQuestion isPlayerClose = new TreeQuestion(IsPlayerClose, claw, groundedRoulette);

			Dictionary<INode, int> flyingOutcomesWithChance = new Dictionary<INode, int>()
															{
																{diagonalFireball, 40},
																{flyingDive, 40},
																{land, 20}
															};
			TreeRoulette flyingRoulette = new TreeRoulette(flyingOutcomesWithChance);

			TreeQuestion amIGrounded = new TreeQuestion(AmIGrounded, isPlayerClose, flyingRoulette);

			IQuestion[] questions = {groundedRoulette, isPlayerClose, flyingRoulette, amIGrounded};
			TreeAction<string>[] actions = {fly, land, claw, flyingDive, horizontalFireball, diagonalFireball};
			_tree = new Tree<string>(questions, actions, OnTreeResponse, amIGrounded);

			//------ Action behaviours  ------
			_actionsWithBehaviours = new Dictionary<string, Action>()
									{
										{FlyKey, actor.TakeOff},
										{LandKey, actor.Land},
										{MeleeKey, () => actor.Act(meleeAction)},
										{FlyingDiveKey, () => actor.Act(flyingDiveAction)},
										{HorizontalFireball, () => actor.Act(throwFireballAction)},
										{DiagonalFireball, () => actor.Act(throwFireballWithAngleAction)}
									};

			//------ Subscribe to the character Can Act event  ------
			actor.OnCanAct += _tree.RunTree;
		}

		private bool AmIGrounded() => _actor.IsGrounded();

		private bool IsPlayerClose() => _meleeAction.CanHit(_actor, new Collider2D[2]);

		private void OnTreeResponse(string answer)
		{
			if (answer != MeleeKey && answer == _lastTreeAnswer)
			{
				_tree.RunTree();
				return;
			}

			_lastTreeAnswer = answer;
			Debug.Log($"{_actor.transform.name}: This character's tree has decided to do: {answer}");
			if (_actionsWithBehaviours.TryGetValue(answer, out Action behaviour))
				behaviour();
			else
				throw new ArgumentOutOfRangeException("The answer given by the decision tree" +
													" wasn't found in the aciton behaviours");
		}
	}
}