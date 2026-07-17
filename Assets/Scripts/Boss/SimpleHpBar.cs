using PolarityBreach.Boss;
using UnityEngine;
using UnityEngine.UI;

namespace PolarityBreach.Boss
{
    public class SimpleHpBar : MonoBehaviour
    {
        [SerializeField] private BossHealth bossHealth;
        [SerializeField] private Slider healthSlider;

        private void Awake()
        {
            if (healthSlider == null)
            {
                healthSlider = GetComponent<Slider>();
            }
        }

        private void OnEnable()
        {
            if (bossHealth != null)
            {
                bossHealth.OnHealthPercentChanged += UpdateHealthBar;
            }
        }

        private void OnDisable()
        {
            if (bossHealth != null)
            {
                bossHealth.OnHealthPercentChanged -= UpdateHealthBar;
            }
        }

        private void Start()
        {
            UpdateHealthBar(1f);
        }

        private void UpdateHealthBar(float healthPercent)
        {
            healthSlider.value = healthPercent;
        }
    }
}
