using System;
using Core.Helpers;
using UnityEngine;

namespace LifeSystem
{
	public class DealDamageOnCollision : MonoBehaviour
	{
		[SerializeField]
		private int damage;

		private void OnCollisionEnter2D(Collision2D col)
		{
			if (col.gameObject.TryGetComponent(out IDamageable damageable))
			{
				damageable.TakeDamage(damage);
			}
		}
	}
}