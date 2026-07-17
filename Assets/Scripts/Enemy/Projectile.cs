using PolarityBreach.Audio;
using PolarityBreach.Player;
using PolarityBreach.PolaritySystem;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

namespace PolarityBreach.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 4f;
        [SerializeField] private AudioClip _impactSound;

        private Rigidbody rb;
        private float speed;
        private float damage;
        private PolarityComponent _polarity;
        private float _spawnTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _polarity = GetComponent<PolarityComponent>();
            rb.useGravity = false;
        }
        
        private void OnEnable()
        {
            _spawnTime = Time.time;
        }

        public void Launch(Vector3 direction, float launchSpeed, float launchDamage)
        {
            speed = launchSpeed;
            damage = launchDamage;
            rb.linearVelocity = direction.normalized * speed;
        }
        
        private void Update()
        {
            if (Time.time - _spawnTime >= lifetime)
                Deactivate();
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Enemy")) return;
            if (!other.CompareTag("Player")) return;
            bool hit = DamageSystem.TryApplyDamage(_polarity, other.gameObject, damage);
            
            if (hit)
            {
                AudioHandler.Play3DSound(_impactSound, transform.position);
                Deactivate();
                return;
            }
            
            IPolarizable otherPolarity = other.GetComponent<IPolarizable>();
            if (otherPolarity != null)
            {
                return; 
            }
        
            if (!other.isTrigger)
            {
                Deactivate();
            }
        }
        
        private void Deactivate()
        {
            rb.linearVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}