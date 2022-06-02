using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicUIManager : MonoBehaviour
{
    private enum SkipPromptState
    {
        HIDDEN,
        SHOWING,
        SHOWN,
        HIDING
    }

    public Image cinematicDarkVeil;
    [SerializeField] private CanvasGroup skipPromptContainer;
    [SerializeField] private Image skipCircle;
    [SerializeField] private float promptFadeTime;
    [SerializeField] private float timeToHidePrompt;

    private SkipPromptState currentSkipPromptState;
    private float currentTime = 0f;
    private float timeSinceLastInteraction = 0f;

    private bool isHoldingSkip = false;
    private const float SKIP_HOLD_TIME = 2f;
    private float currentSkipHoldTime = 0f;

    private void Update()
    {
        switch (currentSkipPromptState)
        {
            case SkipPromptState.HIDDEN:
                break;

            case SkipPromptState.SHOWING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(1f, currentTime / promptFadeTime);

                    skipPromptContainer.alpha = progress;

                    if (progress == 1f)
                    {
                        currentSkipPromptState = SkipPromptState.SHOWN;
                    }
                }
                break;

            case SkipPromptState.SHOWN:
                timeSinceLastInteraction += Time.deltaTime;
                if (timeSinceLastInteraction >= timeToHidePrompt)
                {
                    HideSkipPrompt();
                }
                break;

            case SkipPromptState.HIDING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(1f, currentTime / promptFadeTime);

                    skipPromptContainer.alpha = 1f - progress;

                    if (progress == 1f)
                    {
                        currentSkipPromptState = SkipPromptState.HIDDEN;
                        skipPromptContainer.gameObject.SetActive(false);
                    }
                }
                break;
        }

        if (isHoldingSkip)
        {
            currentSkipHoldTime += Time.deltaTime;
            float progress = Mathf.Min(1f, currentSkipHoldTime / SKIP_HOLD_TIME);

            skipCircle.fillAmount = progress;

            if (progress == 1f)
            {
                isHoldingSkip = false;
            }
        }
    }

    public void ShowSkipPrompt()
    {
        timeSinceLastInteraction = 0f;

        if (currentSkipPromptState == SkipPromptState.SHOWING || currentSkipPromptState == SkipPromptState.SHOWN)
        {
            return;
        }

        skipPromptContainer.gameObject.SetActive(true);

        currentSkipPromptState = SkipPromptState.SHOWING;
        currentTime = 0f;
    }

    private void HideSkipPrompt()
    {
        currentSkipPromptState = SkipPromptState.HIDING;
        currentTime = 0f;
    }
    
    public void StartSkipCinematic()
    {
        isHoldingSkip = true;
        currentSkipHoldTime = 0f;
    }

    public void EndSkipCinematic()
    {
        isHoldingSkip = false;
        skipCircle.fillAmount = 0f;
    }

    public void HideCinematicUI()
    {
        currentSkipPromptState = SkipPromptState.HIDING;
        skipPromptContainer.gameObject.SetActive(false);
        skipPromptContainer.alpha = 0f;

        cinematicDarkVeil.SetAlpha(0f);
    }
}
