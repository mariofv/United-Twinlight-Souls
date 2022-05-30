using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LockUI : UIElement
{
    [SerializeField] private LockCursorUI lockCursorUI;
    [SerializeField] private LockOnIndicatorUI lockOnIndicatorUI;

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

    public void SoftLockEnemy(Transform enemyTransform)
    {
        TargetEnemy(enemyTransform);
        lockCursorUI.SetLockType(LockCursorUI.LockType.SOFT_LOCK);
    }

    public void LockEnemy(Transform enemyTransform)
    {
        TargetEnemy(enemyTransform);
        lockCursorUI.SetLockType(LockCursorUI.LockType.LOCK);
        lockOnIndicatorUI.Show();
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

    public void UnlockEnemy(bool hasToHideUI)
    {
        enemyLocked = false;
        lockCursorUI.HideCursor();
        if (lockCursorUI.GetLockType() == LockCursorUI.LockType.LOCK && hasToHideUI)
        {
            lockOnIndicatorUI.Hide();
        }
    }
}
