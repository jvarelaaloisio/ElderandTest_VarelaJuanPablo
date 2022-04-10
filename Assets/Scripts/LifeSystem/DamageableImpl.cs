using System;
using Core.Helpers;
using Events.UnityEvents;
using LS;
using UnityEngine;
using UnityEngine.Events;

namespace LifeSystem
{
    public class DamageableImpl : MonoBehaviour, IDamageable
    {
        private Damageable _damageable;
        [SerializeField]
        private int maxLifePoints;

        [SerializeField]
        private bool allowOverflow;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDeath;

        [SerializeField]
        private IntUnityEvent onLifeChanged;

        private void Awake()
        {
            _damageable = new Damageable(maxLifePoints, maxLifePoints, allowOverflow);
            _damageable.OnDeath += OnDeath;
        }

        public void TakeDamage(int damage)
        {
            _damageable.TakeDamage(damage);
            onLifeChanged.Invoke(LifePoints);
        }

        public event Action OnDeath;

        public int LifePoints => _damageable.LifePoints;
        public int MaxLifePoints => _damageable.MaxLifePoints;
    }
}
