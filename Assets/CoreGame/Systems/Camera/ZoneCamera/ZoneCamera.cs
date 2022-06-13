using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoneCamera : MonoBehaviour
{
    public CinemachineVirtualCamera zoneVirtualCamera;
    [SerializeField] private CameraRail zoneCameraRail;
    [SerializeField] private Transform zoneCameraLookAt;
    private CinemachineTrackedDolly levelTrackedDolly;

    private void Awake()
    {
        levelTrackedDolly = zoneVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zoneCameraRail == null || GameManager.instance.GetCurrentGameState() == GameManager.GameState.MAIN_MENU)
        {
            return;
        }
        float pathProgress = zoneCameraRail.GetRailProgress();
        UpdateZoneCamera(pathProgress);
    }

    public void UpdateZoneCamera(float progress)
    {
        zoneCameraLookAt.position = zoneCameraRail.GetFocusPosition(progress);
        levelTrackedDolly.m_PathPosition = progress;
    }
}
