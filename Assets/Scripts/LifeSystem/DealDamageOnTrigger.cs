using Core.Helpers;
using UnityEngine;

namespace LifeSystem
{
	public class DealDamageOnTrigger : MonoBehaviour
	{
		[SerializeField]
		private int damage;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IDamageable damageable))
			{
				damageable.TakeDamage(damage);
			}
		}
	}
}