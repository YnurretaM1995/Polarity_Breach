using UnityEngine;

namespace PolarityBreach
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifeTime = 3f;

        private Vector3 direction;
        private float lifeTimer;

        public void Shoot(Vector3 shootDirection)
        {
            direction = shootDirection.normalized;
            lifeTimer = lifeTime;
        }

        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);

            lifeTimer -= Time.deltaTime;

            if (lifeTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Kill();
                Destroy(gameObject);
            }
        }
    }
}