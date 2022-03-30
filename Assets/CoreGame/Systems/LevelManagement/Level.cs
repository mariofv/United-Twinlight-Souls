using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Special Transforms")]
    public Transform spawnedEntitiesHolder;
    public Transform startPosition;
    public Transform voidPosition;
    
    [Header("Zones")]
    public List<Zone> levelZones;
    private int currentZone = 0;

    public CameraRail GetCurrentZoneCameraRail()
    {
        return levelZones[currentZone].cameraRail;
    }

    public void AdvanceZone()
    {
        ++currentZone;
        GameManager.instance.cameraManager.SetCurrentCameraRail(GetCurrentZoneCameraRail());
    }
}
