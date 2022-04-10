using System;

namespace IA.DecisionTree
{
	public interface IQuestion
	{
		event Action<INode> ChangeNode;
	}
}