using System.Collections;

namespace Core.Actors
{
	public interface IFlyingAction
	{
		string Name { get; }
		IEnumerator Behaviour(IFlyingActor flyingActor, IFlyingActorModel model);
	}
}