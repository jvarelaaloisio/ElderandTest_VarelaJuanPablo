using System;
using System.Collections.Generic;
using Actors;
using Actors.Actions;
using Core.Actors;
using IA.DecisionTree;
using Paths;
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

		private Tree<string> _tree;
		private string _lastTreeAnswer;

		private readonly Dictionary<string, Action> _actionsWithBehaviours;
		private readonly Melee _meleeAction;
		private readonly IFlyingAction _flyingDiveAction;
		private readonly IAction _throwHorizontalFireballAction;
		private readonly IAction _throwDiagonalFireballAction;

		private readonly bool _shouldLogTreeDecisions;
		private PathTraveler _pathTraveler;

		public GargoyleAIController(FlyingActor actor,
									PathTraveler pathTraveler,
									Melee meleeAction,
									IFlyingAction flyingDiveAction,
									IAction throwHorizontalFireballAction,
									IAction throwDiagonalFireballAction,
									bool shouldLogTreeDecisions)
		{
			_actor = actor;
			_pathTraveler = pathTraveler;
			_meleeAction = meleeAction;
			_flyingDiveAction = flyingDiveAction;
			_throwHorizontalFireballAction = throwHorizontalFireballAction;
			_throwDiagonalFireballAction = throwDiagonalFireballAction;
			_shouldLogTreeDecisions = shouldLogTreeDecisions;
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
										{FlyKey, () => actor.TakeOff(pathTraveler.Fly(1 / actor.Model.TakeOffSpeed))},
										{LandKey, () => actor.Land(pathTraveler.Land(1 / actor.Model.LandingSpeed))},
										{
											MeleeKey,
											() => actor.Act(_meleeAction,
															pathTraveler
																.GoToNextPoint(1 / actor.Model.MoveSpeed))
										},
										{
											FlyingDiveKey, () => actor.Act(_flyingDiveAction)
										},
										{
											HorizontalFireball,
											() => actor.Act(_throwHorizontalFireballAction,
															pathTraveler
																.GoToNextPoint(1 / actor.Model.MoveSpeed))
										},
										{
											DiagonalFireball,
											() => actor.Act(_throwDiagonalFireballAction,
															pathTraveler
																.GoToNextPoint(1 / actor.Model.MoveSpeed))
										}
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
			if (_shouldLogTreeDecisions)
				Debug.Log($"{_actor.transform.name}: This character's tree has decided to do: {answer}");
			if (_actionsWithBehaviours.TryGetValue(answer, out Action behaviour))
				behaviour();
			else
				throw new ArgumentOutOfRangeException("The answer given by the decision tree" +
													" wasn't found in the aciton behaviours");
		}
	}
}