using System.Collections;

namespace Core.Actors
{
	public interface IAction
	{
		string Name { get; }
		IEnumerator Behaviour(IActor flyingActor, IActorModel model);
	}
}