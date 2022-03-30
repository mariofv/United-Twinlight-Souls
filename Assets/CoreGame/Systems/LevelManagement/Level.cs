using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    [Header("Special Transforms")]
    public Transform spawnedEntitiesHolder;
    public Transform startPosition;
    public Transform voidPosition;
    
    [Header("Zones")]
    public List<Zone> levelZones;
    private int currentZone = 0;

    public CinemachineVirtualCamera GetCurrentZoneCamera()
    {
        return levelZones[currentZone].zoneCamera.zoneVirtualCamera;
    }

    public void AdvanceZone()
    {
        ++currentZone;
        GameManager.instance.cameraManager.LoadCamera(GetCurrentZoneCamera());
    }
}
