using PolarityBreach.Enemy;
using UnityEngine;


namespace PolarityBreach.Boss
{
    public class BossPhaseController : MonoBehaviour
    {
        [SerializeField] private BossPhaseOneAttack phaseOneAttack;

        private EnemyHealth health;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
        }

        private void Start()
        {
            phaseOneAttack.StartPhase();
        }
    }
}
