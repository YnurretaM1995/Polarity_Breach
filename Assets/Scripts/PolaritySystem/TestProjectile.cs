using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    
    [RequireComponent(typeof(PolarityComponent))]
    public class TestProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 12f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifeTime = 3f;

        private PolarityComponent _polarity;
        private float _spawnTime;

        private void Awake() => _polarity = GetComponent<PolarityComponent>();
        private void OnEnable() => _spawnTime = Time.time;

        private void Update()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
            if (Time.time - _spawnTime >= _lifeTime) gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool hit = DamageSystem.TryApplyDamage(_polarity, other.gameObject, _damage);
            if (hit) gameObject.SetActive(false); 
        }
    }
}