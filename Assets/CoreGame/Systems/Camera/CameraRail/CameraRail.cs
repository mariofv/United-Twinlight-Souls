using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    [SerializeField] private CameraLine cameraPositionLine;
    [SerializeField] private CameraLine cameraFocusLine;
    [SerializeField] private CameraLine levelPathLine;

    public void GetCurrentCameraMotion(out Vector3 cameraPosition, out Vector3 cameraFocus)
    {
        float currentProgressInLevel = levelPathLine.GetProgress(GameManager.instance.player.GetPosition());
        cameraFocus = cameraFocusLine.GetPoint(currentProgressInLevel);
        cameraPosition = cameraPositionLine.GetPoint(currentProgressInLevel);
    }
}
