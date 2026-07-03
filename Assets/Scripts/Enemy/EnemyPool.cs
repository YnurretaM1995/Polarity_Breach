using System.Collections.Generic;
using UnityEngine;

namespace PolarityBreach.Enemy
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int poolSize = 20;

        private List<Enemy> enemies = new List<Enemy>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, transform);
                enemy.gameObject.SetActive(false);
                enemies.Add(enemy);
            }
        }

        public Enemy GetEnemy(Vector3 spawnPosition)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].gameObject.activeInHierarchy)
                {
                    enemies[i].transform.position = spawnPosition;
                    enemies[i].gameObject.SetActive(true);
                    return enemies[i];
                }
            }

            Debug.Log("No inactive enemy available in the pool.");
            return null;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}