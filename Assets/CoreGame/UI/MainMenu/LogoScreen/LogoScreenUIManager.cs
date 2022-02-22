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
    [SerializeField] private CanvasGroup logoMenuContainer;
    [SerializeField] private UICustomButton playCustomButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button systemButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    private bool isLogoMenuOpened = false;

    private void Awake()
    {
        pressAnyButtonButton.onClick.AddListener(OnAnyKeyButtonPressed);
        playButton.onClick.AddListener(OnPlayButtonClicked);
        controlsButton.onClick.AddListener(OnControlsButtonClicked);
        systemButton.onClick.AddListener(OnSystemButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.MainMenuUISettings.LOGO_DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    public override void ShowSpecialized(bool instant)
    {
        pressAnyButtonCustomButton.SelectWithoutSound();
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);
    }

    private void OpenLogoMenu()
    {
        pressAnyButtonContainer.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f);
        pressAnyButtonButton.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME);

        logoMenuContainer.gameObject.SetActive(true);
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

    public void OnControlsButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.CONTROLS);
    }

    public void OnSystemButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.SETTINGS);
    }

    public void OnCreditsButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.CREDITS);
    }

    public void OnExitButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.CloseGame();
    }
}
