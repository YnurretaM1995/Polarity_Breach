using System;
using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;

namespace PolarityBreach.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private BloodOverlay bloodOverlay;
        [SerializeField] private CameraControlScript cameraShake;
        
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
            if (playerStats.godMode) return;
            if (IsDead) return;
            if (GodMode) return;

            currentHealth  -= amount;
            Debug.Log($"Life: { currentHealth }");
            
            if (bloodOverlay != null) bloodOverlay.OnDamaged();
            if (cameraShake != null) cameraShake.Shake(1.25f);

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