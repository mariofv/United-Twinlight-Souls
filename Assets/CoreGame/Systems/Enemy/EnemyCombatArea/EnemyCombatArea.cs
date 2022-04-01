using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyCombatArea : MonoBehaviour
{
    [System.Serializable]
    private struct CombatAreaWave
    {
        public List<EnemySpawnPoint> waveSpawnPoints;
    }

    [SerializeField] private CinemachineVirtualCamera combatAreaCamera;
    [SerializeField] private BoxCollider combatAreaEnterGateCollider;
    [SerializeField] private List<CombatAreaWave> combatAreaWaves;

    private bool started = false;
    private int currentWave = 0;
    private int currentWaveEnemies = 0;
    private void StartCombatArea()
    {
        started = true;
        currentWave = 0;
        combatAreaEnterGateCollider.enabled = true;
        GameManager.instance.cameraManager.LoadCamera(combatAreaCamera);
        StartCurrentWave();
    }

    private void EndCombatArea()
    {
        GameManager.instance.levelManager.GetCurrentLevel().AdvanceZone();
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
        else if (currentWave < combatAreaWaves.Count)
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
