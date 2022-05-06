using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class BossHealthBarUI : UIElement
{
    [SerializeField] private RectTransform fillTransform;
    private float originalWidth;

    void Awake()
    {
        originalWidth = fillTransform.rect.width;
    }

    public void InflictDamage(float currentHealthPercentage, float previousHealthPercentage)
    {
        float transformRight = (1 - currentHealthPercentage) * originalWidth;
        fillTransform.SetRight(transformRight);
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
}
