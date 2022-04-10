namespace Core.Actors
{
	public interface IFlyingActorModel : IActorModel
	{
		float TakeOffSpeed { get; }
		float LadingSpeed { get; }
		float FlyingDiveSpeed { get; }
		float FireballSpeed { get; }
		float FireballDamage { get; }
		float ClawDamage { get; }
		float FlyingDiveDamage { get; }
	}
}