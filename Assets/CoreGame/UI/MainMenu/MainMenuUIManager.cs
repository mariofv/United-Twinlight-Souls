using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [System.Serializable]
    private struct MainMenuScreen
    {
        public Transform screenCameraTransform;
        public UIElement screenUIManager;
    }

    [SerializeField] private List<MainMenuScreen> mainMenuScreens;

    // Start is called before the first frame update
    void Start()
    {
        OpenMainMenuScreen(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenuScreen(int screenIndex)
    {
        GameManager.instance.cameraManager.mainCamera.transform.position = mainMenuScreens[screenIndex].screenCameraTransform.position;
        GameManager.instance.cameraManager.mainCamera.transform.rotation = mainMenuScreens[screenIndex].screenCameraTransform.rotation;

        mainMenuScreens[screenIndex].screenUIManager.Show();
    }
}
