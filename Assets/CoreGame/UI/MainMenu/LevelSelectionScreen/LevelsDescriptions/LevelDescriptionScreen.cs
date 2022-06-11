using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Tweening;

public class LevelDescriptionScreen : MonoBehaviour
{
    public enum LevelDescriptionScreenState
    {
        CLOSED,
        OPENING,
        OPENED,
        CLOSING
    }

    [SerializeField] private CanvasGroup levelDescriptionContainer;
    [SerializeField] private Image titleBloom;
    [SerializeField] private float bloomSpeed;
    [SerializeField] private CinemachineVirtualCamera levelCamera;

    private LevelDescriptionScreenState currentState;
    private float currentTime = 0f;

    private void Update()
    {
        titleBloom.SetAlpha((Mathf.Sin(Time.unscaledTime * bloomSpeed) + 1) * 0.25f);

        switch (currentState)
        {
            case LevelDescriptionScreenState.CLOSED:
                break;
            case LevelDescriptionScreenState.OPENING:
                {
                    currentTime += Time.unscaledDeltaTime;
                    float progress = Mathf.Min(1f, currentTime / UISettings.GameUISettings.DISPLAY_TIME);

                    levelDescriptionContainer.alpha = progress;

                    if (progress == 1f)
                    {
                        currentState = LevelDescriptionScreenState.OPENED;
                    }
                }
                break;
            case LevelDescriptionScreenState.OPENED:
                break;
            case LevelDescriptionScreenState.CLOSING:
                {
                    currentTime += Time.unscaledDeltaTime;
                    float progress = Mathf.Min(1f, currentTime / UISettings.GameUISettings.DISPLAY_TIME);

                    levelDescriptionContainer.alpha = 1f - progress;

                    if (progress == 1f)
                    {
                        currentState = LevelDescriptionScreenState.CLOSED;
                        levelDescriptionContainer.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public void Open()
    {
        currentTime = 0f;

        GameManager.instance.cameraManager.LoadCamera(levelCamera);

        levelDescriptionContainer.gameObject.SetActive(true);
        currentState = LevelDescriptionScreenState.OPENING;
    }

    public void Close()
    {
        currentTime = 0f;

        currentState = LevelDescriptionScreenState.CLOSING;
    }

}
