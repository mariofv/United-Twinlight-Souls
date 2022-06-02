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
    [SerializeField] private float promptFadeTime;
    [SerializeField] private float timeToHidePrompt;

    private SkipPromptState currentSkipPromptState;
    private float currentTime = 0f;
    private float timeSinceLastInteraction = 0f;

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
                        timeSinceLastInteraction = 0f;
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
    }

    public void ShowSkipPrompt()
    {
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
    
    public void HideSkipPromptInstantly()
    {
        currentSkipPromptState = SkipPromptState.HIDING;
        skipPromptContainer.gameObject.SetActive(false);
        skipPromptContainer.alpha = 0f;
    }

    public void StartSkipCinematic()
    {

    }

    public void EndSkipCinematic()
    {

    }
}
