using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LockCursorUI : UIElement
{
    public enum LockType
    {
        SOFT_LOCK,
        LOCK,
        HIT_LOCK
    }

    [SerializeField] private RectTransform lockCursorRectTransform;
    [SerializeField] private GameObject softLockCursor;
    [SerializeField] private GameObject lockCursor;
    [SerializeField] private GameObject hitLockCursor;

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

    public void SetLockType(LockType lockType)
    {
        switch (lockType)
        {
            case LockType.SOFT_LOCK:
                softLockCursor.SetActive(true);
                lockCursor.SetActive(false);
                hitLockCursor.SetActive(false);
                break;

            case LockType.LOCK:
                softLockCursor.SetActive(false);
                lockCursor.SetActive(true);
                hitLockCursor.SetActive(false);
                break;

            case LockType.HIT_LOCK:
                softLockCursor.SetActive(false);
                lockCursor.SetActive(false);
                hitLockCursor.SetActive(true);
                break;
        }
    }

    public void SetPosition(Vector2 screenPosition)
    {
        lockCursorRectTransform.position = screenPosition;
    }

    public void ShowCursor()
    {
        uiElementCanvasGroup.gameObject.SetActive(true);
    }

    public void HideCursor()
    {
        uiElementCanvasGroup.gameObject.SetActive(false);
    }

    public bool IsCursorVisible()
    {
        return uiElementCanvasGroup.gameObject.activeSelf;
    }
}
