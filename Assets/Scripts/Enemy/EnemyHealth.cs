using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;
    private float currentHealth;

    private EnemyPool pool;

    private void Awake()
    {
        pool = GetComponentInParent<EnemyPool>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (pool != null)
        {
            pool.ReturnEnemy(this);
        }
        else
        {
            Debug.LogWarning($"[{name}] No EnemyPool found in parent hierarchy — destroying instead.", this);
            Destroy(gameObject);
        }
    }
}