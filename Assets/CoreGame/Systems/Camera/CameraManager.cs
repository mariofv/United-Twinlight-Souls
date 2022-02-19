using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraRail currentCameraRail;
    public Camera mainCamera;

    private Vector3 currentProjectedFront;
    private Vector3 currentProjectedRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetCurrentGameState() == GameManager.GameState.COMBAT)
        {
            Vector3 cameraFocus;
            Vector3 cameraPosition;

            currentCameraRail.GetCurrentCameraMotion(out cameraPosition, out cameraFocus);
            mainCamera.transform.position = cameraPosition;
            mainCamera.transform.LookAt(cameraFocus, Vector3.up);

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
    }
}
