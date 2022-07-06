using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tweening;

public class LogoScreenUIManager : MainMenuScreenUIManager
{
    [SerializeField] private Image logoImage;

    [Header("Logo")]
    [SerializeField] private UICustomButton pressAnyButtonCustomButton;
    [SerializeField] private Button pressAnyButtonButton;
    [SerializeField] private CanvasGroup pressAnyButtonContainer;

    [Header("Logo menu")]
    [SerializeField] private CanvasGroup keyHelperContainer;
    [SerializeField] private CanvasGroup logoMenuContainer;
    [SerializeField] private UICustomButton playCustomButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button systemButton;
    [SerializeField] private Button exitButton;

    private bool isLogoMenuOpened = false;

    private void Awake()
    {
        pressAnyButtonButton.onClick.AddListener(OnAnyKeyButtonPressed);
        playButton.onClick.AddListener(OnPlayButtonClicked);
        systemButton.onClick.AddListener(OnSystemButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public override void ShowSpecialized(bool instant)
    {
        uiElementCanvasGroup.gameObject.SetActive(true);
        if (!isLogoMenuOpened)
        {
            uiElementCanvasGroup.TweenFade(UISettings.MainMenuUISettings.LOGO_DISPLAY_TIME, 0f, 1f);
            pressAnyButtonCustomButton.SelectWithoutSound();
        }
        else
        {
            OpenLogoMenu();
        }
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd();
        hideTweens.Add(disableGameObjectAnimation);
    }

    private void OpenLogoMenu()
    {
        pressAnyButtonContainer.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f);
        pressAnyButtonButton.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME);

        logoMenuContainer.gameObject.SetActive(true);
        keyHelperContainer.gameObject.SetActive(true);
        logoMenuContainer.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f);

        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        playCustomButton.SelectWithoutSound();

        GameManager.instance.inputManager.RestoreSubmitAction();
        isLogoMenuOpened = true;
    }

    public override void OnAnyKeyPressed()
    {
        pressAnyButtonButton.onClick.Invoke();
    }

    public void OnAnyKeyButtonPressed()
    {
        if (!isLogoMenuOpened)
        {
            OpenLogoMenu();
        }
    }

    public void OnPlayButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.SELECT_CHARACTER);
    }

    public void OnSystemButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.SETTINGS);
    }

    public void OnExitButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.CloseGame();
    }
}
