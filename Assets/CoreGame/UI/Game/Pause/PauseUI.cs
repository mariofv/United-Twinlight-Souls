using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class PauseUI : UIElement
{
    [SerializeField] private UICustomButton resumeButton;

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
}
