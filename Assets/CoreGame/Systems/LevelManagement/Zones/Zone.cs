using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zone : MonoBehaviour
{
    public ZoneCamera zoneCamera;

    public void StartZone()
    {

    }

    public void EndZone()
    {

    }

    public CinemachineVirtualCamera GetZoneVirtualCamera()
    {
        return zoneCamera.zoneVirtualCamera;
    }
}
