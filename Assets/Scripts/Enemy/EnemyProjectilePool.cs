using System.Collections.Generic;
using UnityEngine;

namespace PolarityBreach.Enemy
{
    public class EnemyProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private int poolSize = 30;

        private List<Projectile> projectiles = new List<Projectile>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Projectile p = Instantiate(projectilePrefab, transform);
                p.gameObject.SetActive(false);
                projectiles.Add(p);
            }
        }

        public Projectile GetProjectile(Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].gameObject.activeInHierarchy)
                {
                    projectiles[i].transform.SetPositionAndRotation(position, rotation);
                    projectiles[i].gameObject.SetActive(true);
                    return projectiles[i];
                }
            }
            Debug.LogWarning("No enemy projectil on the pool.");
            return null;
        }
    }
}