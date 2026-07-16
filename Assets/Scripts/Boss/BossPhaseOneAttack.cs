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
        
        private float attackStartAngle;
        private bool useInstantCircle = true;
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
            if (_phaseRoutine != null)
            {
                StopCoroutine(_phaseRoutine);
            }
        }

        private Polarity GetRandomPolarity()
        {
            return Random.value < 0.5d ? Polarity.White : Polarity.Black;
        }

        private IEnumerator PhaseOneLoop()
        {
            while (true)
            {
                Polarity randomPolarity = GetRandomPolarity();

                if (useInstantCircle)
                {
                    FireCircleInstant(randomPolarity);
                }
                else
                {
                    yield return StartCoroutine(FireCircleSequence(randomPolarity));
                }

                useInstantCircle = !useInstantCircle;

                yield return new WaitForSeconds(phaseData.timeBetweenAttacks);
            }
        }

        private void FireCircleInstant(Polarity polarity)
        {
            for (int i = 0; i < phaseData.instantBulletCount; i++)
            {
                float angle = attackStartAngle + 360f / phaseData.instantBulletCount * i;
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                FireProjectile(direction, polarity, phaseData.instantProjectileSpeed, phaseData.instantProjectileDamage);
            }

            attackStartAngle += phaseData.attackStartAngleOffset;
        }

        private IEnumerator FireCircleSequence(Polarity polarity)
        {
            for (int i = 0; i < phaseData.sequenceBulletCount; i++)
            {
                float angle = attackStartAngle + ((360f / phaseData.sequenceBulletCount) * i);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                FireProjectile(direction, polarity, phaseData.sequenceProjectileSpeed, phaseData.sequenceProjectileDamage);

                yield return new WaitForSeconds(phaseData.sequenceBulletDelay);
            }

            attackStartAngle += phaseData.attackStartAngleOffset;
        }
        
        private void FireProjectile(Vector3 direction, Polarity polarity, float projectileSpeed, float projectileDamage)
        {
            if (projectilePool == null)
            {
                return;
            }

            Projectile projectile = projectilePool.GetProjectile(firePoint.position, Quaternion.LookRotation(direction));

            if (projectile == null) return;

            PolarityComponent projectilePolarity = projectile.GetComponent<PolarityComponent>();

            if (projectilePolarity != null)
            {
                projectilePolarity.SetPolarity(polarity);
            }

            projectile.Launch(direction, projectileSpeed, projectileDamage);
        }
    }
}
