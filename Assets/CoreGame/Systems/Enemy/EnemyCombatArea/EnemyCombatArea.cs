using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatArea : MonoBehaviour
{
    [System.Serializable]
    private struct CombatAreaWave
    {
        public List<EnemySpawnPoint> waveSpawnPoints;
    }

    [SerializeField] private List<CombatAreaWave> combatAreaWaves;

    private bool started = false;
    private int currentWave = 0;
    private int currentWaveEnemies = 0;

    // Start is called before the first frame updat
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartCombatArea()
    {
        currentWave = 0;
        StartCurrentWave();
    }

    private void EndCombatArea()
    {

    }

    private void StartCurrentWave()
    {
        List<EnemySpawnPoint> currentWaveSpawnPoints = combatAreaWaves[currentWave].waveSpawnPoints;
        for (int i = 0; i < currentWaveSpawnPoints.Count; ++i)
        {
            Enemy waveEnemy = currentWaveSpawnPoints[i].Spawn();
            waveEnemy.onSpawnedEnemyDead.AddListener(OnWaveEnemyDead);
            ++currentWaveEnemies;
        }
    }

    private void EndCurrentWave()
    {
        ++currentWave;
        if (currentWave == combatAreaWaves.Count)
        {
            EndCombatArea();
        }
        else if (currentWave > combatAreaWaves.Count)
        {
            StartCurrentWave();
        }
        else
        {
            throw new UnityException("Current wave counter is higher than the number of waves!");
        }
    }

    private void OnWaveEnemyDead()
    {
        --currentWaveEnemies;
        if (currentWaveEnemies == 0)
        {
            EndCurrentWave();
        }
        else if (currentWaveEnemies < 0)
        {
            throw new UnityException("Number of current wave enemies is negative!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER) && !started)
        {
            StartCombatArea();
        }
    }
}
