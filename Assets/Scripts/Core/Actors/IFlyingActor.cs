using System;
using System.Collections;

namespace Core.Actors
{
	public interface IFlyingActor : IActor
	{
		event Action OnTakeOff;
		event Action OnLanding;
		void Act(IFlyingAction flyingAction, IEnumerator afterBehaviour = null);
		void TakeOff(IEnumerator takeOffBehaviour);
		void Land(IEnumerator landBehaviour);
		bool IsGrounded();
	}
}