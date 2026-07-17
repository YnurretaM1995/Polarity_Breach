using PolarityBreach.PolaritySystem;
using UnityEngine;

namespace PolarityBreach.Boss
{
    [CreateAssetMenu(fileName = "BossPhaseData", menuName = "Scriptable Objects/Boss Phase Data")]
    public class BossPhaseData : ScriptableObject
    {
        [Header("Phase Info")]
        public string phaseName = "Phase 1";

        [Header("Shared Attack Settings")]
        public float timeBetweenAttacks = 2f;
        public float attackStartAngleOffset = 35f;
        public bool randomizePolarity = true;
        public Polarity fallbackPolarity = Polarity.White;
        
        [Header("Instant Circle Attack")]
        public int instantBulletCount = 25;
        public float instantProjectileSpeed = 10f;
        public float instantProjectileDamage = 20f;

        [Header("Sequence Circle Attack")]
        public int sequenceBulletCount = 50;
        public float sequenceProjectileSpeed = 500f;
        public float sequenceProjectileDamage = 10f;
        public float sequenceBulletDelay = 0.2f;
    }
    
    
}
