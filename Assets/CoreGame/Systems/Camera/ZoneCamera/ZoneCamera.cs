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
        if (GameManager.instance.GetCurrentGameState() == GameManager.GameState.COMBAT)
        {
            float pathProgress = zoneCameraRail.GetRailProgress();
            zoneCameraLookAt.position = zoneCameraRail.GetFocusPosition(pathProgress);
            levelTrackedDolly.m_PathPosition = pathProgress;

            GameManager.instance.cameraManager.UpdateCameraVectors();
        }
    }
}
