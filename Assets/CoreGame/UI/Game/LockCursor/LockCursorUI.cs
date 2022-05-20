using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LockCursorUI : UIElement
{
    [SerializeField] private RectTransform cursorTransform;

    private bool enemyLocked = false;
    private Transform lockedEnemyTransform;

    private void Update()
    {
        if (enemyLocked)
        {
            Vector2 cursorScreenPosition = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(lockedEnemyTransform.position);
            cursorTransform.position = cursorScreenPosition;
        }
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

    public void LockOnEnemy(Transform enemyTransform)
    {
        enemyLocked = true;
        lockedEnemyTransform = enemyTransform;
        cursorTransform.gameObject.SetActive(true);
    }

    public void UnlockEnemy()
    {
        enemyLocked = false;
        cursorTransform.gameObject.SetActive(false);
    }
}
