using System.Collections;
using UnityEngine;

namespace PolarityBreach.Enemy
{
    public enum SpawnPattern
    {
        RandomCluster,
        LineShape,
        VShape,
        CircleShape,
        zigzagShape
    }

    [System.Serializable]
    public class EnemySpawnGroup
    {
        public string name = "Group";
        public int enemyCount = 5;
        public SpawnPattern pattern = SpawnPattern.RandomCluster;
        public float spacing = 2f;
    }

    [System.Serializable]
    public class EnemyWave
    {
        public string name = "Wave";
        public EnemySpawnGroup[] groups;
    }

    public class EnemyWaveSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private Transform[] possibleSpawnPoints;

        [Header("Wave Settings")]
        [SerializeField] private EnemyWave[] waves;
        [SerializeField] private float timeBetweenSpawns = 0.3f;
        [SerializeField] private float timeBetweenWaves = 3f;
        [SerializeField] private float enemySpawnHeight = 0.5f;

        [Header("Random Cluster Settings")]
        [SerializeField] private float clusterRadius = 3f;

        private int aliveEnemies;

        private void Start()
        {
            StartCoroutine(RunWaves());
        }

        private IEnumerator RunWaves()
        {
            for (int waveIndex = 0; waveIndex < waves.Length; waveIndex++)
            {
                Debug.Log("Starting wave " + (waveIndex + 1));

                yield return StartCoroutine(SpawnWave(waves[waveIndex]));

                yield return new WaitUntil(() => aliveEnemies <= 0);

                Debug.Log("Wave " + (waveIndex + 1) + " completed");

                yield return new WaitForSeconds(timeBetweenWaves);
            }

            Debug.Log("Room cleared!");
        }

        private IEnumerator SpawnWave(EnemyWave wave)
        {
            for (int groupIndex = 0; groupIndex < wave.groups.Length; groupIndex++)
            {
                EnemySpawnGroup group = wave.groups[groupIndex];
                Transform spawnPoint = GetRandomSpawnPoint();

                if (spawnPoint == null)
                {
                    Debug.LogWarning("No spawn points assigned.");
                    yield break;
                }

                Vector3[] offsets = GetPatternOffsets(group.pattern, group.enemyCount, group.spacing);

                for (int i = 0; i < offsets.Length; i++)
                {
                    Vector3 spawnPosition = spawnPoint.position + offsets[i];
                    spawnPosition.y = enemySpawnHeight;

                    SpawnEnemyAtPosition(spawnPosition);

                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }
        }

        private Transform GetRandomSpawnPoint()
        {
            if (possibleSpawnPoints == null || possibleSpawnPoints.Length == 0)
                return null;
            

            int randomIndex = Random.Range(0, possibleSpawnPoints.Length);
            return possibleSpawnPoints[randomIndex];
        }

        private void SpawnEnemyAtPosition(Vector3 spawnPosition)
        {
            Enemy enemy = enemyPool.GetEnemy(spawnPosition);

            if (enemy == null)
            {
                Debug.LogWarning("EnemyPool did not return an enemy.");
                return;
            }

            aliveEnemies++;
            enemy.Spawn(this);
        }

        private Vector3[] GetPatternOffsets(SpawnPattern pattern, int enemyCount, float spacing)
        {
            if (pattern == SpawnPattern.LineShape)
                return GetLineOffsets(enemyCount, spacing);

            return GetRandomClusterOffsets(enemyCount);
        }

        private Vector3[] GetRandomClusterOffsets(int enemyCount)
        {
            Vector3[] offsets = new Vector3[enemyCount];

            for (int i = 0; i < enemyCount; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * clusterRadius;
                offsets[i] = new Vector3(randomCircle.x, 0f, randomCircle.y);
            }

            return offsets;
        }
        
        private Vector3[] GetLineOffsets(int enemyCount, float spacing)
        {
            Vector3[] offsets = new Vector3[enemyCount];

            float startX = -(enemyCount - 1) * spacing * 0.5f;

            for (int i = 0; i < enemyCount; i++)
            {
                float x = startX + i * spacing;
                offsets[i] = new Vector3(x, 0f, 0f);
            }

            return offsets;
        }

        public void EnemyDied(Enemy enemy)
        {
            aliveEnemies--;

            enemyPool.ReturnEnemy(enemy);
        }
    }
}