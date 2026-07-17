using PolarityBreach.Audio;
using PolarityBreach.Feedback;
using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    
    [RequireComponent(typeof(PolarityComponent))]
    public class ShootProjectile : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 3f;
        [SerializeField] private bool _disapearOnHit = true;
        [SerializeField] private GameObject _impactEffect;
        [SerializeField] private AudioClip _impactSound;
        
        private float _speed;
        private float _damage;
        private float _knockbackForce;
        
        private PolarityComponent _polarity;
        private float _spawnTime;

        private void Awake() => _polarity = GetComponent<PolarityComponent>();
        private void OnEnable() => _spawnTime = Time.time;

        private void Update()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
            if (Time.time - _spawnTime >= _lifeTime) gameObject.SetActive(false);
        }

        public void SetStats(float speed, float damage, float knockbackForce)
        {
            _speed = speed;
            _damage = damage;
            _knockbackForce = knockbackForce;
        }

        private void OnTriggerEnter(Collider other)
        {
            bool hit = DamageSystem.TryApplyDamage(_polarity, other.gameObject, _damage);
            if (hit)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                Vector3 impactDirection = transform.forward;
                FeedbackHandler.SpawnParticles(_impactEffect, transform.position,impactDirection);
                AudioHandler.Play3DSound(_impactSound, transform.position);

                if (rb != null)
                {
                    Vector3 knockbackDirection = transform.forward;
                    knockbackDirection.y = 0;
                    knockbackDirection.Normalize();
                    
                    rb.AddForce(knockbackDirection * _knockbackForce, ForceMode.Impulse);
                }

                if (_disapearOnHit)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
