using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraRail currentCameraRail;
    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFocus;
        Vector3 cameraPosition;

        currentCameraRail.GetCurrentCameraMotion(out cameraPosition, out cameraFocus);
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.LookAt(cameraFocus, Vector3.up);
    }

    public void SetCurrentCameraRail(CameraRail newCameraRail)
    {
        currentCameraRail = newCameraRail;
    }

}
