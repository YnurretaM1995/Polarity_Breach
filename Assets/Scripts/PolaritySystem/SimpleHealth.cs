using System;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

namespace PolarityBreach.PolaritySystem
{

    public class SimpleHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _maxHealth = 30f;
        private float _current;

        public event Action OnDied;
        public event Action<float, float> OnHealthChanged; 

        public float Current => _current;
        public float Max => _maxHealth;

        private void Awake() => _current = _maxHealth;
        private void OnEnable() => _current = _maxHealth; 

        public void TakeDamage(float amount)
        {
            if (_current <= 0f) return;

            _current = Mathf.Max(0f, _current - amount);
            OnHealthChanged?.Invoke(_current, _maxHealth);
            Debug.Log($"{name} took {amount} damage. HP: {_current}/{_maxHealth}");

            if (_current <= 0f)
            {
                _current = 0f;
                OnDied?.Invoke();
            }
        }
    }
}