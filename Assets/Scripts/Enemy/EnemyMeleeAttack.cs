using System.Collections;
using UnityEngine;

/// <summary>
/// Minimal melee "swing" attack. Enables a hitbox child object for a brief
/// window when the enemy is in range, then disables it and waits on cooldown.
///
/// SETUP:
/// 1. On EnemyPursuitAI, uncheck "Is Ranged Enemy" and set "Stopping Distance"
///    to roughly match "Attack Range" below (e.g. 1.5) so the enemy stops at
///    swing range instead of walking into the player.
/// 2. Create a child GameObject on the enemy called "SwingHitbox":
///    - Add a BoxCollider, set "Is Trigger" ON, size/position it to cover
///      the swing area in front of the enemy (e.g. Size (1.5, 1, 1), Center (0.75, 0, 0)).
///    - Add EnemyMeleeHitbox.cs to it.
///    - Leave it enabled in the prefab; this script disables it on Awake.
/// 3. Add this script to the enemy, assign "Swing Hitbox" to that child object.
/// 4. If you had EnemyContactDamage on this enemy, remove it — the swing
///    hitbox replaces plain contact damage.
/// </summary>
[RequireComponent(typeof(EnemyPursuitAI))]
public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject swingHitbox;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f; // should roughly match EnemyPursuitAI's Stopping Distance
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float windupTime = 0.2f;   // brief pause before the hitbox activates (telegraph)
    [SerializeField] private float swingDuration = 0.15f; // how long the hitbox stays active

    private EnemyPursuitAI pursuitAI;
    private float cooldownTimer;
    private bool isAttacking;

    private void Awake()
    {
        pursuitAI = GetComponent<EnemyPursuitAI>();
        if (swingHitbox != null) swingHitbox.SetActive(false);
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isAttacking) return; // don't interrupt a swing in progress
        if (cooldownTimer > 0f) return;
        if (!CanAttack()) return;

        StartCoroutine(DoSwing());
    }

    private bool CanAttack()
    {
        if (pursuitAI.Target == null) return false;
        if (!pursuitAI.IsEngaged) return false;
        if (!pursuitAI.CanSeeTarget) return false;

        float dist = Vector3.Distance(transform.position, pursuitAI.Target.position);
        return dist <= attackRange;
    }

    private IEnumerator DoSwing()
    {
        isAttacking = true;

        yield return new WaitForSeconds(windupTime);

        if (swingHitbox != null) swingHitbox.SetActive(true);

        yield return new WaitForSeconds(swingDuration);

        if (swingHitbox != null) swingHitbox.SetActive(false);

        cooldownTimer = attackCooldown;
        isAttacking = false;
    }
}