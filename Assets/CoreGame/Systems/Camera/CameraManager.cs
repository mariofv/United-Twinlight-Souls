using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 currentProjectedFront;
    private Vector3 currentProjectedRight;

    [Header("Level Camera")]
    [SerializeField] private CinemachineVirtualCamera levelVirtualCamera;
    [SerializeField] private Transform levelCameraLookAt;
    private CinemachineTrackedDolly levelTrackedDolly;
    private CameraRail currentCameraRail;

    private void Awake()
    {
        levelTrackedDolly = levelVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetCurrentGameState() == GameManager.GameState.COMBAT)
        {
            float pathProgress;
            Vector3 cameraFocus;

            currentCameraRail.GetCurrentCameraMotion(out pathProgress, out cameraFocus);
            levelCameraLookAt.position = cameraFocus;
            levelTrackedDolly.m_PathPosition = pathProgress;

            currentProjectedFront = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            currentProjectedRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up);
        }
    }

    public Vector3 GetCurrentProjectedFront()
    {
        return currentProjectedFront;
    }

    public Vector3 GetCurrentProjectedRight()
    {
        return currentProjectedRight;
    }

    public void SetCurrentCameraRail(CameraRail newCameraRail)
    {
        currentCameraRail = newCameraRail;
        levelTrackedDolly.m_Path = currentCameraRail.GetCameraPositionPath();
    }
}
