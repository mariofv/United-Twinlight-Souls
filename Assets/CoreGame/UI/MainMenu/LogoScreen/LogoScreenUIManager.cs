using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tweening;

public class LogoScreenUIManager : UIElement
{
    [SerializeField] private Image logoImage;
    [SerializeField] private Button pressAnyButtonButton;

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.MainMenuUISettings.LOGO_DISPLAY_TIME, 0f, 1f);
        showTweens.Add(fadeAnimation);
    }

    public override void ShowSpecialized(bool instant)
    {
        EventSystem.current.SetSelectedGameObject(pressAnyButtonButton.gameObject);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f);
        hideTweens.Add(fadeAnimation);
    }

    public void OnAnyKeyPressed()
    {
        GameManager.instance.uiManager.mainMenuUIManager.NextScreen();
    }
}
