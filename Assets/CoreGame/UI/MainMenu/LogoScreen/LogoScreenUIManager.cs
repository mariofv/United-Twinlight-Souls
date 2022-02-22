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

    [SerializeField] private Button pressAnyButtonButton;
    [SerializeField] private CanvasGroup pressAnyButtonContainer;

    [SerializeField] private CanvasGroup logoMenuContainer;

    private bool isLogoMenuOpened = false;

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.MainMenuUISettings.LOGO_DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    public override void ShowSpecialized(bool instant)
    {
        EventSystem.current.SetSelectedGameObject(pressAnyButtonButton.gameObject);
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

        isLogoMenuOpened = true;
    }

    public override void OnAnyKeyPressed()
    {
        if (!isLogoMenuOpened)
        {
            OpenLogoMenu();
        }
    }
}
