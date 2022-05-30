using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIndicatorUI : MonoBehaviour
{
    private enum LockOnState
    {
        HIDDEN,
        SHOWING,
        SHOWN,
        HIDING
    }

    [SerializeField] private CanvasGroup lockOnContainer;
    [SerializeField] private RectTransform indicatorRectTransform;
    [SerializeField] private Vector2 animationOffset;
    
    private Vector2 originalPosition;
    private LockOnState currentState;
    private float currentTime = 0f;

    private void Awake()
    {
        originalPosition = indicatorRectTransform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case LockOnState.HIDDEN:
                break;

            case LockOnState.SHOWING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(currentTime / UISettings.GameUISettings.DISPLAY_TIME, 1f);

                    lockOnContainer.alpha = progress;
                    indicatorRectTransform.position = Vector2.Lerp(originalPosition + animationOffset, originalPosition, progress);

                    if (progress == 1f)
                    {
                        currentState = LockOnState.SHOWN;
                    }
                }
                break;

            case LockOnState.SHOWN:
                break;

            case LockOnState.HIDING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(currentTime / UISettings.GameUISettings.DISPLAY_TIME, 1f);

                    lockOnContainer.alpha = 1f - progress;
                    indicatorRectTransform.position = Vector2.Lerp(originalPosition, originalPosition + animationOffset, progress);

                    if (progress == 1f)
                    {
                        currentState = LockOnState.HIDDEN;
                        lockOnContainer.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public void Show()
    {
        if (currentState == LockOnState.SHOWING || currentState == LockOnState.SHOWN)
        {
            return;
        }

        currentState = LockOnState.SHOWING;
        currentTime = 0f;
        lockOnContainer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (currentState == LockOnState.HIDDEN || currentState == LockOnState.HIDING)
        {
            return;
        }

        currentState = LockOnState.HIDING;
        currentTime = 0f;
    }
}
