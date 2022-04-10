using System;

namespace Core.Helpers
{
	public interface IDamageable
	{
		void TakeDamage(int damage);
		int LifePoints { get; }
		int MaxLifePoints { get; }
		event Action OnDeath;
	}
}
