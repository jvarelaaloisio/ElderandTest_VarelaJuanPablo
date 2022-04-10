using System;
using Core.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class SimpleProjectile : MonoBehaviour
	{
		[Header("Game Design")]
		[SerializeField]
		private float speed;

		[SerializeField]
		private float lifeTime;

		[SerializeField]
		private int damage;

		[Header("Setup")]
		[SerializeField]
		private float delayBeforeDestroyingAfterHit;
		
		[SerializeField]
		private Rigidbody2D rigidbody2D;

		[Header("Events")]
		[SerializeField]
		private UnityEvent onHit;

		private void OnValidate()
		{
			if (!rigidbody2D)
				rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void Awake()
		{
			Invoke(nameof(Die), lifeTime);
			rigidbody2D.velocity = transform.right * speed;
		}

		private void OnCollisionEnter2D(Collision2D col)
		{
			if (col.gameObject.TryGetComponent(out IDamageable damageable))
			{
				damageable.TakeDamage(damage);
			}

			rigidbody2D.velocity = Vector3.zero;
			CancelInvoke(nameof(Die));
			Invoke(nameof(Die), lifeTime);
			onHit.Invoke();
		}

		private void Die()
		{
			Destroy(gameObject);
		}
	}
}