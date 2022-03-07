using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainMenuScreenUIManager : UIElement
{
    public enum MainMenuScreenId
    {
        NONE,
        LOGO,
        SELECT_CHARACTER,
        SELECT_LEVEL,
        CONTROLS,
        SETTINGS,
        CREDITS
    }

    public MainMenuScreenId mainMenuScreenId;
    public Transform cameraTransform;

    public virtual void OnAnyKeyPressed() { }
    public virtual void OnRightPressed() { }
    public virtual void OnLeftPressed() { }
    public virtual void OnConfirmPressed() { }
    public virtual void OnCancelPressed() { }
}
