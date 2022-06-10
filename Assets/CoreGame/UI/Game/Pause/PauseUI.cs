using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Tweening;

public class PauseUI : UIElement
{
    [SerializeField] private UICustomButton resumeButton;
    [SerializeField] private SystemScreenUIManager systemsMenu;

    private bool isSystemsMenuOpen = false;

    public override void ShowSpecialized(bool instant)
    {
        resumeButton.SelectWithoutSound();
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd().Unscaled();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd().Unscaled();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd().Unscaled();
        hideTweens.Add(disableGameObjectAnimation);
    }

    public void OnResumeButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.ResumeGame();
    }

    public void OnSystemsButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        OpenSystemsMenu();
    }

    public void OnMainMenuButtonClicked()
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.CLICK);
        GameManager.instance.InitMainMenu();
    }

    private void OpenSystemsMenu()
    {
        systemsMenu.Show();
        Hide(instant: true);
        isSystemsMenuOpen = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseSystemsMenu()
    {
        systemsMenu.Hide();
        Show(instant: true);
        isSystemsMenuOpen = false;
    }

    public bool IsSystemsMenuOpen()
    {
        return isSystemsMenuOpen;
    }

    public void OnUpPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnUpPressed();
    }

    public void OnDownPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnDownPressed();
    }

    public void OnRightPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnRightPressed();
    }

    public void OnLeftPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnLeftPressed();
    }

    public void OnNextTabPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnNextTabPressed();
    }

    public void OnPreviousTabPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }

        systemsMenu.OnPreviousTabPressed();
    }

    public void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (!IsSystemsMenuOpen())
        {
            return;
        }
        CloseSystemsMenu();
    }
}
