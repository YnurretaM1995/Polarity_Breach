using UnityEngine;

[RequireComponent(typeof(EnemyPursuitAI))]
public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Firing")]
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float maxFireRange = 12f;
    [SerializeField] private int damage = 10;

    private EnemyPursuitAI pursuitAI;
    private float fireCooldown;
    private Collider[] ownColliders;

    private void Awake()
    {
        pursuitAI = GetComponent<EnemyPursuitAI>();
        if (firePoint == null) firePoint = transform;
        ownColliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        if (!CanFire()) return;

        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / Mathf.Max(fireRate, 0.01f);
        }
    }

    private bool CanFire()
    {
        if (pursuitAI.Target == null) return false;
        if (!pursuitAI.IsEngaged) return false;
        if (!pursuitAI.CanSeeTarget) return false;

        float dist = Vector3.Distance(transform.position, pursuitAI.Target.position);
        return dist <= maxFireRange;
    }

    private void Fire()
    {
        if (projectilePrefab == null || pursuitAI.Target == null) return;

        Vector3 targetPoint = pursuitAI.Target.position + Vector3.up * 0.5f;
        Vector3 dir = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));

        Collider projCollider = proj.GetComponent<Collider>();
        if (projCollider != null)
        {
            foreach (Collider ownCollider in ownColliders)
            {
                if (ownCollider != null)
                    Physics.IgnoreCollision(projCollider, ownCollider);
            }
        }

        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Launch(dir, projectileSpeed, damage);
        }
    }
}