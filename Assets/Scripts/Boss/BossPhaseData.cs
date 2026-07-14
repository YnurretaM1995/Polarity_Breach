using PolarityBreach.PolaritySystem;
using UnityEngine;

namespace PolarityBreach.Boss
{
    [CreateAssetMenu(fileName = "BossPhaseData", menuName = "Scriptable Objects/Boss Phase Data")]
    public class BossPhaseData : ScriptableObject
    {
        [Header("Phase Info")]
        public string phaseName = "Phase 1";

        [Header("Circle Projectile Attack")]
        public int bulletCount = 25;
        public float projectileSpeed = 10f;
        public float projectileDamage = 10f;
        public float projectileFireRate = 2f;
        public Polarity attackPolarity = Polarity.White;
    }
}
