using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsTab : MonoBehaviour
{
    public enum TabState
    {
        CLOSED,
        OPENING,
        OPENED,
        CLOSING
    }

    private Image tabImage;
    [SerializeField] private CanvasGroup tabContent;

    private TabState currentState;
    private float currentTime = 0f;

    private void Awake()
    {
        tabImage = GetComponent<Image>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case TabState.CLOSED:
                break;
            case TabState.OPENING:
                {
                    currentTime += Time.unscaledDeltaTime;
                    float progress = Mathf.Min(1f, currentTime / UISettings.GameUISettings.DISPLAY_TIME);

                    tabContent.alpha = progress;

                    if (progress == 1f)
                    {
                        currentState = TabState.OPENED;
                    }
                }
                break;
            case TabState.OPENED:
                break;
            case TabState.CLOSING:
                {
                    currentTime += Time.unscaledDeltaTime;
                    float progress = Mathf.Min(1f, currentTime / UISettings.GameUISettings.DISPLAY_TIME);

                    tabContent.alpha = 1f - progress;

                    if (progress == 1f)
                    {
                        currentState = TabState.CLOSED;
                        tabContent.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public void Open()
    {
        tabImage.SetAlpha(1f);
        currentTime = 0f;

        tabContent.gameObject.SetActive(true);
        currentState = TabState.OPENING;
    }

    public void Close()
    {
        tabImage.SetAlpha(0f);
        currentTime = 0f;

        currentState = TabState.CLOSING;
    }
}
