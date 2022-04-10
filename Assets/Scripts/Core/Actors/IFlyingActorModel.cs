namespace Core.Actors
{
	public interface IFlyingActorModel : IActorModel
	{
		float TakeOffSpeed { get; }
		float LandingSpeed { get; }
	}
}