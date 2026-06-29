using UnityEngine;

// TEMPORARY ENEMY CLASS

public class Enemy : MonoBehaviour
{
    private EnemyWaveSpawner waveSpawner;
    [SerializeField] private bool isDead;

    public void Spawn(EnemyWaveSpawner spawner)
    {
        waveSpawner = spawner;
        isDead = false;
    }

    [ContextMenu("Kill Enemy")]
    public void Kill()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (waveSpawner != null)
        {
            waveSpawner.EnemyDied(this);
        }
    }
}
