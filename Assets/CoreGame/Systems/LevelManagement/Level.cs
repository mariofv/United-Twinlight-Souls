using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    [Header("Special Transforms")]
    public Transform startPosition;
    public Transform voidPosition;
    
    [Header("Zones")]
    [SerializeField] private List<Zone> levelZones;
    private int currentZone = 0;

    public CinemachineVirtualCamera GetCurrentZoneCamera()
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
        GameManager.instance.cameraManager.LoadCamera(GetCurrentZoneCamera());
    }
}
