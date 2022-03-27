using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRail : MonoBehaviour
{
    [SerializeField] private CameraLine cameraPositionLine;
    [SerializeField] private CameraLine cameraFocusLine;
    [SerializeField] private CameraLine levelPathLine;

    public void GetCurrentCameraMotion(out float pathProgress, out Vector3 cameraFocus)
    {
        //TODO: This may be inneficient, maybe cache the player transform
        pathProgress = levelPathLine.GetProgress(GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition());
        cameraFocus = cameraFocusLine.GetPoint(pathProgress);
    }

    public CinemachineSmoothPath GetCameraPositionPath()
    {
        return cameraPositionLine.cinemachinePath;
    }
}
