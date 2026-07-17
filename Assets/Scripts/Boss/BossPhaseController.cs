using PolarityBreach.Enemy;
using UnityEngine;


namespace PolarityBreach.Boss
{
    public class BossPhaseController : MonoBehaviour
    {
        [SerializeField] private BossPhaseOneAttack phaseOneAttack;

        private BossHealth health;

        private void Awake()
        {
            health = GetComponent<BossHealth>();
        }

        private void Start()
        {
            phaseOneAttack.StartPhase();
        }
    }
}
