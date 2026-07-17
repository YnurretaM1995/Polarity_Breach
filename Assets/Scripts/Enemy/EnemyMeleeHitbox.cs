using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;

/// <summary>
/// The actual damage-dealing hitbox for a melee swing. Lives on a child
/// GameObject that EnemyMeleeAttack enables/disables briefly during a swing.
/// Requires a Collider set to "Is Trigger".
/// </summary>
[RequireComponent(typeof(Collider))]
public class EnemyMeleeHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}