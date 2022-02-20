using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LoadingScreenUIManager : UIElement
{
    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).Unscaled().DontKillOnEnd();
        showTweens.Add(fadeAnimation); ;
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).Unscaled().DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).Unscaled().DontKillOnEnd();
        hideTweens.Add(disableAnimation);
    }
}
