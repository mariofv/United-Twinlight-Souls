using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private List<MainMenuScreenUIManager> mainMenuScreensManagers;
    private Dictionary<MainMenuScreenUIManager.MainMenuScreenId, MainMenuScreenUIManager> mainMenuScreens;

    private MainMenuScreenUIManager.MainMenuScreenId currentMainMenuScrenId = MainMenuScreenUIManager.MainMenuScreenId.NONE;

    [SerializeField] private Transform baraldTransform;

    private void Awake()
    {
        mainMenuScreens = new Dictionary<MainMenuScreenUIManager.MainMenuScreenId, MainMenuScreenUIManager>();
        for (int i = 0; i < mainMenuScreensManagers.Count; ++i)
        {
            mainMenuScreens.Add(mainMenuScreensManagers[i].mainMenuScreenId, mainMenuScreensManagers[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.player.GetBarald().Teleport(baraldTransform.position);
        GameManager.instance.player.GetBarald().Orientate(baraldTransform.rotation);

        OpenMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId.LOGO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId mainMenuScreenId)
    {
        if (currentMainMenuScrenId != MainMenuScreenUIManager.MainMenuScreenId.NONE)
        {
            CloseMainMenuScreen(currentMainMenuScrenId);
        }

        currentMainMenuScrenId = mainMenuScreenId;

        GameManager.instance.cameraManager.mainCamera.transform.position = mainMenuScreens[mainMenuScreenId].cameraTransform.position;
        GameManager.instance.cameraManager.mainCamera.transform.rotation = mainMenuScreens[mainMenuScreenId].cameraTransform.rotation;

        mainMenuScreens[mainMenuScreenId].Show();
    }

    public void CloseMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId mainMenuScreenId)
    {
        mainMenuScreens[mainMenuScreenId].Hide();
    }

    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnAnyKeyPressed();
    }
}
