using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyCombatArea : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera combatAreaCamera;
    [SerializeField] private Checkpoint combatAreaCheckpoint;
    [SerializeField] private BoxCollider combatAreaEnterGateCollider;
    [SerializeField] private List<EnemyWave> combatAreaWaves;

    private bool started = false;
    private int currentWave = 0;

    private void StartCombatArea()
    {
        started = true;
        currentWave = 0;
        combatAreaEnterGateCollider.enabled = true;
        GameManager.instance.levelManager.GetCurrentLevelAsZoned().SetCurrentCombatArea(this);
        GameManager.instance.cameraManager.LoadCamera(combatAreaCamera);
        StartCurrentWave();
    }

    private void EndCombatArea()
    {
        combatAreaCheckpoint.ActivateCheckpoint();
        GameManager.instance.levelManager.GetCurrentLevelAsZoned().AdvanceZone();
    }

    public void ResetCombatArea()
    {
        started = false;
        combatAreaEnterGateCollider.enabled = false;
        combatAreaWaves[currentWave].onWaveEnd.RemoveAllListeners();
    }

    private void StartCurrentWave()
    {
        combatAreaWaves[currentWave].StartWave();
        combatAreaWaves[currentWave].onWaveEnd.AddListener(EndCurrentWave);
    }

    private void EndCurrentWave()
    {
        combatAreaWaves[currentWave].onWaveEnd.RemoveAllListeners();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER) && !started)
        {
            StartCombatArea();
        }
    }
}
