using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class ControlsScreenUIManager : MainMenuScreenUIManager
{
    public override void ShowSpecialized(bool instant)
    {
        uiElementCanvasGroup.gameObject.SetActive(true);
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd();
        hideTweens.Add(disableGameObjectAnimation);
    }

    public override void OnCancelPressed()
    {
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.LOGO);
    }
}
