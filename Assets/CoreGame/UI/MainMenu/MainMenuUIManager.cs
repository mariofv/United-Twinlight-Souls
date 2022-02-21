using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuUIManager : MonoBehaviour
{
    [System.Serializable]
    private struct MainMenuScreen
    {
        public Transform screenCameraTransform;
        public UIElement screenUIManager;
    }

    [SerializeField] private List<MainMenuScreen> mainMenuScreens;
    int currentMainMenuScreen;

    // Start is called before the first frame update
    void Start()
    {
        OpenMainMenuScreen(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScreen()
    {
        if (currentMainMenuScreen + 1 < mainMenuScreens.Count)
        {
            CloseMainMenuScren(currentMainMenuScreen);
            ++currentMainMenuScreen;

            OpenMainMenuScreen(currentMainMenuScreen);
        }
    }

    public void PreviousScreen()
    {
        if (currentMainMenuScreen - 1 >= 0)
        {
            CloseMainMenuScren(currentMainMenuScreen);
            --currentMainMenuScreen;

            OpenMainMenuScreen(currentMainMenuScreen);
        }
    }

    private void OpenMainMenuScreen(int screenIndex)
    {
        GameManager.instance.cameraManager.mainCamera.transform.position = mainMenuScreens[screenIndex].screenCameraTransform.position;
        GameManager.instance.cameraManager.mainCamera.transform.rotation = mainMenuScreens[screenIndex].screenCameraTransform.rotation;

        mainMenuScreens[screenIndex].screenUIManager.Show();
    }

    public void CloseMainMenuScren(int screenIndex)
    {
        mainMenuScreens[screenIndex].screenUIManager.Hide();
    }

    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        ((LogoScreenUIManager)mainMenuScreens[0].screenUIManager).OnAnyKeyPressed();
    }
}
