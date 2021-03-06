using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IA.DecisionTree
{
	public class TreeRoulette : INode, IQuestion
	{
		public event Action<INode> ChangeNode;
		private readonly Dictionary<INode, int> _outcomesWithChance;

		public TreeRoulette(Dictionary<INode, int> outcomesWithChance)
		{
			_outcomesWithChance = outcomesWithChance;
		}

		public void Execute()
		{
			int maxWeight = _outcomesWithChance
				.Aggregate(0, (acum, chance) => acum + chance.Value);
			int random = Random.Range(0, maxWeight);
			foreach (var current in _outcomesWithChance)
			{
				random -= current.Value;
				if (random >= 0)
					continue;
				ChangeNode(current.Key);
				return;
			}

			throw new ArgumentOutOfRangeException("outcomesWithChance",
												"Roulette couldn't make a decision");
		}
	}
}