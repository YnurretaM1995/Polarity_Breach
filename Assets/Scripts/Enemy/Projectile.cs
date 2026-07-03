using PolarityBreach.PolaritySystem;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

namespace PolarityBreach.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 4f;

        private Rigidbody rb;
        private float speed;
        private float damage;
        private PolarityComponent _polarity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _polarity = GetComponent<PolarityComponent>();
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
            if (!other.CompareTag("Player")) return;
            bool hit = DamageSystem.TryApplyDamage(_polarity, other.gameObject, damage);
            
            if (hit)
            {
                Destroy(gameObject);
                return;
            }
            
            IPolarizable otherPolarity = other.GetComponent<IPolarizable>();
            if (otherPolarity != null)
            {
                return; 
            }
        
            if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}