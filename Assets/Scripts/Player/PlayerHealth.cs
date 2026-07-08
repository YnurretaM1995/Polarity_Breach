using System;
using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine.AdaptivePerformance;

namespace PolarityBreach.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        private PlayerStatsData playerStats;
        private float currentHealth;
        public bool GodMode { get; set; } = false;

        public event Action OnDied;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => playerStats.maxHealth;
        public bool IsDead => currentHealth <= 0f;
        
        private void Awake()
        {
            playerStats = GetComponent<PlayerStatsData>();
        }

        private void OnEnable()
        {
            currentHealth = playerStats.maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            if (GodMode) return;

            currentHealth -= amount;
            Debug.Log($"Life: {currentHealth}");

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                OnDied?.Invoke();
            }
        }
        
        public void FullHeal()
        {
            currentHealth = playerStats.maxHealth;
        }
    }
}