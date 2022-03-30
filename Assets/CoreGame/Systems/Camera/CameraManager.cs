using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 currentProjectedFront;
    private Vector3 currentProjectedRight;

    [Header("MainMenu Cameras")]
    [SerializeField] private CinemachineVirtualCamera logoCamera;
    [SerializeField] private CinemachineVirtualCamera characterSelectionCamera;

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
            float pathProgress = currentCameraRail.GetRailProgress();
            levelCameraLookAt.position = currentCameraRail.GetFocusPosition(pathProgress);
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

    public void LoadLevelCamera()
    {
        UnloadAllCameras();
        levelVirtualCamera.enabled = true;
    }

    public void LoadMainMenuCamera(MainMenuScreenUIManager.MainMenuScreenId mainMenuScreen)
    {
        UnloadAllCameras();
        switch (mainMenuScreen)
        {
            case MainMenuScreenUIManager.MainMenuScreenId.NONE:
                break;
            case MainMenuScreenUIManager.MainMenuScreenId.LOGO:
                logoCamera.enabled = true;
                break;

            case MainMenuScreenUIManager.MainMenuScreenId.SELECT_CHARACTER:
                characterSelectionCamera.enabled = true;
                break;

            case MainMenuScreenUIManager.MainMenuScreenId.SELECT_LEVEL:
                break;
            case MainMenuScreenUIManager.MainMenuScreenId.CONTROLS:
                break;
            case MainMenuScreenUIManager.MainMenuScreenId.SETTINGS:
                break;
            case MainMenuScreenUIManager.MainMenuScreenId.CREDITS:
                break;
        }
    }

    private void UnloadAllCameras()
    {
        logoCamera.enabled = false;
        characterSelectionCamera.enabled = false;

        levelVirtualCamera.enabled = false;
    }

    public void SetCurrentCameraRail(CameraRail newCameraRail)
    {
        currentCameraRail = newCameraRail;
        levelTrackedDolly.m_Path = currentCameraRail.GetCameraPositionPath();
    }
}
