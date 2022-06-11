using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private List<MainMenuScreenUIManager> mainMenuScreensManagers;
    private Dictionary<MainMenuScreenUIManager.MainMenuScreenId, MainMenuScreenUIManager> mainMenuScreens;

    private MainMenuScreenUIManager.MainMenuScreenId currentMainMenuScrenId = MainMenuScreenUIManager.MainMenuScreenId.NONE;

    [SerializeField] private Transform baraldTransform;
    [SerializeField] private Transform ilonaTransform;

    [SerializeField] private Image darkVeil;
    private float darkVeilTime;
    private float currentTime = 0f;
    private bool darkVeilShown = false;

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

        GameManager.instance.player.GetIlona().Teleport(ilonaTransform.position);
        GameManager.instance.player.GetIlona().Orientate(ilonaTransform.rotation);

        OpenMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId.LOGO);
    }

    private void Update()
    {
        if (darkVeilShown)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(1f, currentTime / darkVeilTime);
            float darkVeilProgress;
            if (progress <= 0.25f)
            {
                darkVeilProgress = progress * 4f;
            }
            else if (progress > 0.75f)
            {
                darkVeilProgress = 1f - (progress - 0.5f) * 4f;
            }
            else
            {
                darkVeilProgress = 1f;
            }

            darkVeil.SetAlpha(Mathf.SmoothStep(0f, 1f, darkVeilProgress));
        }
    }

    public void OpenMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId mainMenuScreenId)
    {
        if (currentMainMenuScrenId != MainMenuScreenUIManager.MainMenuScreenId.NONE)
        {
            CloseMainMenuScreen(currentMainMenuScrenId);
        }

        currentMainMenuScrenId = mainMenuScreenId;

        GameManager.instance.cameraManager.LoadCamera(mainMenuScreens[currentMainMenuScrenId].GetMainMenuScreenCamera());

        mainMenuScreens[mainMenuScreenId].Show();
    }

    public void ShowAndHideDarkVeil(float time)
    {
        currentTime = 0f;
        darkVeilShown = true;
        darkVeilTime = time;
    }

    public void CloseMainMenuScreen(MainMenuScreenUIManager.MainMenuScreenId mainMenuScreenId)
    {
        mainMenuScreens[mainMenuScreenId].Hide();
    }

    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnAnyKeyPressed();
    }

    public void OnUpPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnUpPressed();
    }

    public void OnDownPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnDownPressed();
    }

    public void OnRightPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnRightPressed();
    }

    public void OnLeftPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnLeftPressed();
    }

    public void OnNextTabPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnNextTabPressed();
    }

    public void OnPreviousTabPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnPreviousTabPressed();
    }

    public void OnConfirmPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnConfirmPressed();
    }

    public void OnCancelPressed(InputAction.CallbackContext context)
    {
        mainMenuScreens[currentMainMenuScrenId].OnCancelPressed();
    }
}
