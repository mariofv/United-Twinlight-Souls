using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class MainMenuScreenUIManager : UIElement
{
    public enum MainMenuScreenId
    {
        NONE,
        LOGO,
        SELECT_CHARACTER,
        SELECT_LEVEL,
        SETTINGS,
        CREDITS
    }

    public MainMenuScreenId mainMenuScreenId;
    [SerializeField] private CinemachineVirtualCamera mainMenuScreenVirtualCamera;

    public virtual void OnAnyKeyPressed() { }
    public virtual void OnUpPressed() { }
    public virtual void OnDownPressed() { }
    public virtual void OnRightPressed() { }
    public virtual void OnLeftPressed() { }
    public virtual void OnNextTabPressed() { }
    public virtual void OnPreviousTabPressed() { }
    public virtual void OnConfirmPressed() { }
    public virtual void OnCancelPressed() { }

    public CinemachineVirtualCamera GetMainMenuScreenCamera()
    {
        return mainMenuScreenVirtualCamera;
    }
}
