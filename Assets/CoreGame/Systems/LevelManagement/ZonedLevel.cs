using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZonedLevel : Level
{
    [Header("Zones")]
    [SerializeField] private List<Zone> levelZones;
    private int currentZone = 0;

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

    public void AdvanceZone()
    {
        levelZones[currentZone].zoneExitBarrier.Open();
        ++currentZone;
        GameManager.instance.cameraManager.LoadCamera(GetCurrentCamera());
    }

}
