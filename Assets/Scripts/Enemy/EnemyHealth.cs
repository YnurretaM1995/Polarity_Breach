using System;
using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;

namespace PolarityBreach.Enemy
{

    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 30f;
        private float currentHealth;

        public event Action OnDied;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        private void OnEnable()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            currentHealth -= amount;

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                OnDied?.Invoke();
            }
        }
    }
}