using UnityEngine;
using UnityEngine.UI;

namespace PolarityBreach.Player
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private RectTransform healthBar;
        
        [SerializeField] private Image healthBarImage;

        [SerializeField] private float maxWidth;
        [SerializeField] private float height;
        
        [SerializeField] private Color healthyColor = Color.green;
        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color criticalColor = Color.red;

        private void Update()
        {
            
            float healthPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;
            healthPercentage = Mathf.Clamp01(healthPercentage);
            
            float newWidth = healthPercentage * maxWidth;
            healthBar.sizeDelta = new Vector2(newWidth, height);
            
            UpdateBarColor(healthPercentage);
        }

        private void UpdateBarColor(float percentage)
        {
            if (healthBarImage == null) return;
            
            if (percentage < 0.30f)
            {
                healthBarImage.color = criticalColor;
            }
            else if (percentage < 0.60f)
            {
                healthBarImage.color = warningColor;
            }
            else
            {
                healthBarImage.color = healthyColor;
            }
        }
    }
}