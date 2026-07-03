using UnityEngine;

namespace PolarityBreach
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float timeBetweenShots = 0.2f;

        private float shootTimer;

        private void Update()
        {
            shootTimer -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && shootTimer <= 0f)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            Projectile projectile = Instantiate(
                projectilePrefab,
                shootPoint.position,
                shootPoint.rotation
            );

            projectile.Shoot(shootPoint.forward);

            shootTimer = timeBetweenShots;
        }
    }
}