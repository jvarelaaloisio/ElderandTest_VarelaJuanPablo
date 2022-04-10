using System;

namespace Core.Actors
{
	public interface IFlyingActor : IActor
	{
		event Action OnTakeOff;
		event Action OnLanding;
		void Act(IFlyingAction flyingAction);
		void TakeOff();
		void Land();
		bool IsGrounded();
	}
}