using PolarityBreach.Enemy;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;
using System;

namespace PolarityBreach.Boss
{
    public class BossShield : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private EnemyWaveSpawner enemyWaveSpawner;
        
        private float currentHealth;

        public event Action OnShieldDestroyed;
        
        public bool IsActive => gameObject.activeInHierarchy;

        void OnEnable()
        {
            currentHealth = maxHealth;
        }
        
        public void TakeDamage(float amount)
        {
            if (enemyWaveSpawner != null && enemyWaveSpawner.AliveEnemies > 0)
            {
                Debug.Log("Shield is invulnerable while enemies are alive.");
                return;
            }
            
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            if (currentHealth <= 0f)
            {
                OnShieldDestroyed?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
