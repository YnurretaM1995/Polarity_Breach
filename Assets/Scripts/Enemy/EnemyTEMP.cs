using UnityEngine;
using PolarityBreach.PolaritySystem;

// TEMPORARY ENEMY CLASS

namespace PolarityBreach.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private bool isDead;
        private EnemyWaveSpawner waveSpawner;
        private SimpleHealth _health;

        void Awake()
        {
            _health = GetComponent<SimpleHealth>();
        }

        void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDied += Kill;
            }
        }

        void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDied -= Kill;
            }
        }

        public void Spawn(EnemyWaveSpawner spawner)
        {
            waveSpawner = spawner;
            isDead = false;
        }

        [ContextMenu("Kill Enemy")]
        public void Kill()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;

            if (waveSpawner != null)
            {
                waveSpawner.EnemyDied(this);
            }
        }
    }
}
