using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LockUI : UIElement
{
    [SerializeField] private LockCursorUI lockCursorUI;

    private bool enemyLocked = false;
    private Transform lockedEnemyTransform;

    private void Update()
    {
        if (enemyLocked)
        {
            Vector2 cursorScreenPosition = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(lockedEnemyTransform.position);
            lockCursorUI.SetPosition(cursorScreenPosition);
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

    public void PotentialLockEnemy(Transform enemyTransform)
    {
        TargetEnemy(enemyTransform);
        lockCursorUI.SetLockType(LockCursorUI.LockType.POTENTIAL_LOCK);
    }

    public void LockEnemy(Transform enemyTransform)
    {
        TargetEnemy(enemyTransform);
        lockCursorUI.SetLockType(LockCursorUI.LockType.LOCK);
    }

    private void TargetEnemy(Transform enemyTransform)
    {
        enemyLocked = true;
        lockedEnemyTransform = enemyTransform;

        if (!lockCursorUI.IsCursorVisible())
        {
            lockCursorUI.ShowCursor();
        }
    }

    public void UnlockEnemy()
    {
        enemyLocked = false;
        lockCursorUI.HideCursor();
    }
}
