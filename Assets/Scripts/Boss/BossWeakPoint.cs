using PolarityBreach.PolaritySystem;
using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;

namespace PolarityBreach.Boss
{
    [RequireComponent(typeof(PolarityComponent))]
    public class BossWeakPoint : MonoBehaviour, IDamageable
    {
        [SerializeField] private Collider weakPointCollider;

        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        

        private BossHealth bossHealth;
        private bool isDestroyed;
        
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDestroyed => isDestroyed;

        void Awake()
        {
            if (weakPointCollider == null)
            {
                weakPointCollider = GetComponent<Collider>();
            }
        }
        
        public void Initialize(BossHealth owner, float healthAmount)
        {
            bossHealth = owner;
            maxHealth = healthAmount;
            currentHealth = maxHealth;
            isDestroyed = false;

            if (weakPointCollider != null)
            {
                weakPointCollider.enabled = true;
            }
        }

        public void TakeDamage(float amount)
        {
            if (isDestroyed) return;

            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            bossHealth.RefreshHealth();

            if (currentHealth <= 0f)
            {
                DestroyWeakPoint();
            }
        }
        
        private void DestroyWeakPoint()
        {
            isDestroyed = true;

            if (weakPointCollider != null)
            {
                weakPointCollider.enabled = false;
            }

            bossHealth.WeakPointDestroyed();
            
            Debug.Log(gameObject.name + " weak point destroyed.");
        }
    }
}
