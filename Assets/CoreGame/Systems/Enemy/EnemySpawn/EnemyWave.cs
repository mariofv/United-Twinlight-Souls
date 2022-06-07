using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    public UnityEvent onWaveSpawnEnd;
    public UnityEvent onWaveEnd;
    
    private List<EnemySpawnPoint> waveEnemies;
    private int currentWaveSpawnedEnemies;
    private int currentWaveEnemies;

    private void Awake()
    {
        waveEnemies = new List<EnemySpawnPoint>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            waveEnemies.Add(transform.GetChild(i).GetComponent<EnemySpawnPoint>());
        }
    }

    public void StartWave()
    {
        currentWaveEnemies = waveEnemies.Count;
        currentWaveSpawnedEnemies = waveEnemies.Count;
        for (int i = 0; i < currentWaveEnemies; ++i)
        {
            Enemy spawnedEnemy = waveEnemies[i].Spawn();
            spawnedEnemy.onSpawnEnd.AddListener(OnWaveEnemySpawnEnd);
            spawnedEnemy.onSpawnedEnemyDead.AddListener(OnWaveEnemyDead);
        }
    }

    private void OnWaveEnemySpawnEnd()
    {
        --currentWaveSpawnedEnemies;
        if (currentWaveSpawnedEnemies == 0)
        {
            onWaveSpawnEnd.Invoke();
        }
    }

    private void OnWaveEnemyDead()
    {
        --currentWaveEnemies;
        if (currentWaveEnemies == 0)
        {
            onWaveEnd.Invoke();
        }
    }
}
