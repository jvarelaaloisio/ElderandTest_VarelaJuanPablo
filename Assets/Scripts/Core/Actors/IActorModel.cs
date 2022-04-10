namespace Core.Actors
{
	public interface IActorModel
	{
		float DelayToAct { get; }
		float MoveSpeed { get; }
		float MeleeDistance { get; }
	}
}