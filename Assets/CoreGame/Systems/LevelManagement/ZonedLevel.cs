using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZonedLevel : Level
{
    [Header("Zones")]
    [SerializeField] private List<Zone> levelZones;
    private int currentZone = 0;
    private EnemyCombatArea currentCombatArea = null;

    public override CinemachineVirtualCamera GetCurrentCamera()
    {
        return levelZones[currentZone].zoneCamera.zoneVirtualCamera;
    }

    public int GetNumberOfZones()
    {
        return levelZones.Count;
    }

    public Vector3 GetZonePosition(int zoneIndex)
    {
        return levelZones[zoneIndex].zoneCombatAreaTransform.position;
    }
    public void ResetCombatArea()
    {
        currentCombatArea.ResetCombatArea();
        currentCombatArea = null;
    }

    public bool IsPlayerInCombatArea()
    {
        return currentCombatArea != null;
    }

    public void SetCurrentCombatArea(EnemyCombatArea combatArea)
    {
        currentCombatArea = combatArea;
    }

    public void AdvanceZone()
    {
        currentCombatArea = null;
        levelZones[currentZone].zoneExitBarrier.Open();
        levelZones[currentZone].Purify();
        ++currentZone;
        GameManager.instance.cameraManager.LoadCamera(GetCurrentCamera());
    }
}
