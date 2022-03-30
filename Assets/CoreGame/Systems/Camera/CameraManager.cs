using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    private CinemachineVirtualCamera currentVirtualCamera;

    private Vector3 currentProjectedFront;
    private Vector3 currentProjectedRight;

    public void UpdateCameraVectors()
    {
        currentProjectedFront = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
        currentProjectedRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up);
    }

    public Vector3 GetCurrentProjectedFront()
    {
        return currentProjectedFront;
    }

    public Vector3 GetCurrentProjectedRight()
    {
        return currentProjectedRight;
    }

    public void LoadCamera(CinemachineVirtualCamera camera)
    {
        if (currentVirtualCamera == camera)
        {
            return;
        }

        UnloadCurrentCamera();
        currentVirtualCamera = camera;
        currentVirtualCamera.enabled = true;
    }

    private void UnloadCurrentCamera()
    {
        if (currentVirtualCamera != null)
        {
            currentVirtualCamera.enabled = false;
        }
    }
}
