using UnityEngine;
using PolarityBreach.PolaritySystem;


namespace PolarityBreach.Boss
{
    public class BossPhaseController : MonoBehaviour
    {
        [SerializeField] private BossPhaseOneAttack phaseOneAttack;
        [SerializeField] private BossPhaseTwoAttack phaseTwoAttack;
        [SerializeField] private BossShield bossShield;

        private BossHealth health;
        private PolarityComponent shieldPolarity;
        private int currentPhase = 1;

        private void Awake()
        {
            health = GetComponent<BossHealth>();

            if (phaseOneAttack == null)
            {
                phaseOneAttack = GetComponent<BossPhaseOneAttack>();
            }

            if (phaseTwoAttack == null)
            {
                phaseTwoAttack = GetComponent<BossPhaseTwoAttack>();
            }
            
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

            if (phaseTwoAttack != null)
            {
                phaseTwoAttack.StopPhase();
            }
            
            StartCurrentPhase();
        }
        
        private void StartShieldTransition()
        {
            StopCurrentPhase();
            
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

            if (currentPhase == 1)
            {
                currentPhase = 2;
            }

            StartCurrentPhase();
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnWeakPointDestroyed -= StartShieldTransition;
            }

            if (bossShield != null)
            {
                bossShield.OnShieldDestroyed -= HandleShieldDestroyed;
            }
        }

        private void StopCurrentPhase()
        {
            if (currentPhase == 1)
            {
                if (phaseOneAttack != null)
                {
                    phaseOneAttack.StopPhase();
                }

                return;
            }

            if (currentPhase == 2 && phaseTwoAttack != null)
            {
                phaseTwoAttack.StopPhase();
            }
        }

        private void StartCurrentPhase()
        {
            if (currentPhase == 1)
            {
                if (phaseOneAttack != null)
                {
                    phaseOneAttack.StartPhase();
                }
                else
                {
                    Debug.LogWarning("BossPhaseController is missing Phase One Attack.");
                }

                return;
            }

            if (currentPhase == 2 && phaseTwoAttack != null)
            {
                phaseTwoAttack.StartPhase();
            }
            else if (currentPhase == 2)
            {
                Debug.LogWarning("BossPhaseController is missing Phase Two Attack. Add BossPhaseTwoAttack to the boss and assign it in the inspector.");
            }
        }
    }
}
