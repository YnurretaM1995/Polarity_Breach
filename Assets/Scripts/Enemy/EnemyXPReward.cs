using UnityEngine;

namespace PolarityBreach.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyXPReward : MonoBehaviour
    {
        [SerializeField] private int xpReward = 10;

        private EnemyHealth enemyHealth;

        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnEnable()
        {
            enemyHealth.OnDied += GrantXP;
        }

        private void OnDisable()
        {
            enemyHealth.OnDied -= GrantXP;
        }

        private void GrantXP()
        {
            if (PolarityBreach.Player.PlayerXP.Instance != null)
            {
                PolarityBreach.Player.PlayerXP.Instance.AddXP(xpReward);
            }
        }
    }
}