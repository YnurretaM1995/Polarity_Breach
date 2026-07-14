using System.Collections;
using PolarityBreach.Enemy;
using PolarityBreach.PolaritySystem;
using UnityEngine;

namespace PolarityBreach.Boss
{
    public class BossPhaseOneAttack : MonoBehaviour
    {
        [SerializeField] private EnemyProjectilePool projectilePool;
        [SerializeField] private Transform firePoint;
        
        [SerializeField] private BossPhaseData phaseData;
        
        private Coroutine _phaseRoutine;
        
        public void StartPhase()
        {
            if (phaseData == null)
            {
                return;
            }

            if (firePoint == null)
            {
                firePoint = transform;
            }

            _phaseRoutine = StartCoroutine(PhaseOneLoop());
        }

        public void StopPhase()
        {
            if(_phaseRoutine != null)
            {
                StopCoroutine(_phaseRoutine);
            }
        }

        private IEnumerator PhaseOneLoop()
        {
            while (true)
            {
                FireCircle(phaseData.attackPolarity);

                yield return new WaitForSeconds(phaseData.projectileFireRate);
            }
        }

        private void FireCircle(Polarity polarity)
        {
            for (int i = 0; i < phaseData.bulletCount; i++)
            {
                float angle = (360f / phaseData.bulletCount) * i;
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                FireProjectile(direction, polarity);
            }
        }
        
        private void FireProjectile(Vector3 direction, Polarity polarity)
        {
            if (projectilePool == null)
            {
                return;
            }

            Projectile projectile = projectilePool.GetProjectile(
                firePoint.position,
                Quaternion.LookRotation(direction)
            );

            if (projectile == null) return;

            PolarityComponent projectilePolarity = projectile.GetComponent<PolarityComponent>();

            if (projectilePolarity != null)
            {
                projectilePolarity.SetPolarity(polarity);
            }

            projectile.Launch(direction, phaseData.projectileSpeed, phaseData.projectileDamage);
        }
    }
}
