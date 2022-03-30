using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRail : MonoBehaviour
{
    [SerializeField] private CameraLine cameraPositionLine;
    [SerializeField] private CameraLine cameraFocusLine;
    [SerializeField] private CameraLine levelPathLine;

    public float GetRailProgress()
    {
        //TODO: This may be inneficient, maybe cache the player transform
        return levelPathLine.GetProgress(GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition());
    }

    public Vector3 GetFocusPosition(float railProgress)
    {
        return cameraFocusLine.GetPoint(railProgress);
    }

    public CinemachineSmoothPath GetCameraPositionPath()
    {
        return cameraPositionLine.cinemachinePath;
    }
}
