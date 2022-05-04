using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    public UnityEvent onWaveEnd;
    
    [SerializeField] private List<EnemySpawnPoint> waveEnemies;
    private int currentWaveEnemies;

    public void StartWave()
    {
        currentWaveEnemies = waveEnemies.Count;
        for (int i = 0; i < currentWaveEnemies; ++i)
        {
            Enemy spawnedEnemy = waveEnemies[i].Spawn();
            spawnedEnemy.onSpawnedEnemyDead.AddListener(OnWaveEnemyDead);
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
