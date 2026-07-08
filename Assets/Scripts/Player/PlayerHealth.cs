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
        
        public float MaxHealth => playerStats.maxHealth;
        public bool IsDead => playerStats.maxHealth <= 0f;
        
        private void Awake()
        {
            playerStats = GetComponent<PlayerStatsData>();
        }

        public void TakeDamage(float amount)
        {
            if (playerStats.godMode) return;
            if (IsDead) return;
            if (GodMode) return;

            playerStats.maxHealth -= amount;
            Debug.Log($"Life: {playerStats.maxHealth}");

            if (playerStats.maxHealth <= 0f)
            {
                playerStats.maxHealth = 0f;
                OnDied?.Invoke();
            }
        }
        
        public void FullHeal()
        {
            currentHealth = playerStats.maxHealth;
        }
    }
}