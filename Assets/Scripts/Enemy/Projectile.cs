using UnityEngine;
using PolarityBreach.PolaritySystem.Interfaces;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 4f;

    private Rigidbody rb;
    private float speed;
    private float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Launch(Vector3 direction, float launchSpeed, float launchDamage)
    {
        speed = launchSpeed;
        damage = launchDamage;
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}