using System.Collections;
using PolarityBreach.Enemy;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPool enemyPool;

    [Header("Wave Settings")]
    [SerializeField] private int[] enemiesPerWave = { 5, 8, 12 };
    [SerializeField] private float timeBetweenSpawns = 0.3f;
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float enemySpawnHeight = 0.5f;

    [Header("Random Spawn Settings")]
    [SerializeField] private float spawnAreaRadius = 20f;
    [SerializeField] private float clusterRadius = 3f;

    private int aliveEnemies;
    private Vector3 waveCenter;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (int waveIndex = 0; waveIndex < enemiesPerWave.Length; waveIndex++)
        {
            Debug.Log("Starting wave " + (waveIndex + 1));

            waveCenter = GetRandomPointAround(transform.position, spawnAreaRadius);

            yield return StartCoroutine(SpawnWave(enemiesPerWave[waveIndex]));

            yield return new WaitUntil(() => aliveEnemies <= 0);

            Debug.Log("Wave " + (waveIndex + 1) + " completed");

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Room cleared!");
    }

    private IEnumerator SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomPointAround(waveCenter, clusterRadius);

        Enemy enemy = enemyPool.GetEnemy(spawnPosition);

        if (enemy == null)
        {
            Debug.LogWarning("EnemyPool did not return an enemy.");
            return;
        }

        aliveEnemies++;
        enemy.Spawn(this);
    }

    private Vector3 GetRandomPointAround(Vector3 center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;

        return new Vector3(
            center.x + randomCircle.x, enemySpawnHeight, center.z + randomCircle.y
        );
    }

    public void EnemyDied(Enemy enemy)
    {
        aliveEnemies--;

        enemyPool.ReturnEnemy(enemy);
    }
}