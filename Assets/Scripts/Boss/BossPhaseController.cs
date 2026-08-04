using PolarityBreach.Enemy;
using UnityEngine;
using PolarityBreach.PolaritySystem;


namespace PolarityBreach.Boss
{
    public class BossPhaseController : MonoBehaviour
    {
        [SerializeField] private BossPhaseOneAttack phaseOneAttack;
        [SerializeField] private BossShield bossShield;

        private BossHealth health;
        private PolarityComponent shieldPolarity;

        private void Awake()
        {
            health = GetComponent<BossHealth>();
            
            if (bossShield != null)
            {
                shieldPolarity = bossShield.GetComponent<PolarityComponent>();
            }
        }

        private void Start()
        {
            bossShield.gameObject.SetActive(false);
            health.SetShielded(false);
            
            health.OnWeakPointDestroyed += StartShieldTransition;
            
            phaseOneAttack.StartPhase();
        }
        
        private void StartShieldTransition()
        {
            phaseOneAttack.StopPhase();
            
            health.SetShielded(true);
            
            if (shieldPolarity != null)
            {
                shieldPolarity.Toggle();
            }

            bossShield.gameObject.SetActive(true);
            bossShield.OnShieldDestroyed += HandleShieldDestroyed;
        }

        private void HandleShieldDestroyed()
        {
            bossShield.OnShieldDestroyed -= HandleShieldDestroyed;

            health.SetShielded(false);
            
            phaseOneAttack.StartPhase();

            // Start phase 2 here
        }

        private void OnDestroy()
        {
            health.OnWeakPointDestroyed -= StartShieldTransition;
        }
    }
}
