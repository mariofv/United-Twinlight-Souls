using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class InteractionUI : UIElement
{
    [SerializeField] private Transform interactionIndicatorTransform;
    [SerializeField] private float verticalOffset;
    private Transform targetTransform;
    private bool isTargeting = false;

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

    private void Update()
    {
        if (!isTargeting)
        {
            return;
        }

        Vector2 targetScreenPosition = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(targetTransform.position);
        interactionIndicatorTransform.position = targetScreenPosition + Vector2.up * verticalOffset;
    }

    public void SetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        isTargeting = targetTransform != null;
    }
}
