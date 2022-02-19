using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenDisplayer : MonoBehaviour
{
    private enum SplashScreenState
    {
        Showing,
        Displaying,
        Hiding
    }

    [SerializeField] private float splashScreenDuration;
    [SerializeField] private float transitionDuration;

    [SerializeField] private List<Sprite> splashScreens;
    [SerializeField] private Image splashScreenImage;
    private CanvasGroup splashScreenCanvasGroup;

    private int currentSplashScreen = 0;
    private float currentTime = 0;
    private SplashScreenState currentState;

    private void Awake()
    {
        splashScreenCanvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowCurrentSplashScreen();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        switch (currentState)
        {
            case SplashScreenState.Showing:
                {
                    float progress = Mathf.Min(1f, currentTime / transitionDuration);
                    splashScreenCanvasGroup.alpha = progress;
                    if (progress == 1f)
                    {
                        currentState = SplashScreenState.Displaying;
                        currentTime = 0f;
                    }
                }
                break;

            case SplashScreenState.Displaying:
                {
                    float progress = Mathf.Min(1f, currentTime / splashScreenDuration);
                    if (progress == 1f)
                    {
                        currentState = SplashScreenState.Hiding;
                        currentTime = 0f;
                    }
                }
                break;

            case SplashScreenState.Hiding:
                {
                    float progress = Mathf.Min(1f, currentTime / transitionDuration);
                    splashScreenCanvasGroup.alpha = 1f - progress;
                    if (progress == 1f)
                    {
                        ++currentSplashScreen;
                        if (currentSplashScreen < splashScreens.Count)
                        {
                            ShowCurrentSplashScreen();
                        }
                        else
                        {
                            EndSplashScreenDisplay();
                        }
                    }
                }
                break;
        }
    }

    private void ShowCurrentSplashScreen()
    {
        currentState = SplashScreenState.Showing;
        currentTime = 0f;

        splashScreenCanvasGroup.alpha = 0f;
        splashScreenImage.sprite = splashScreens[currentSplashScreen];
    }

    private void EndSplashScreenDisplay()
    {

    }
}
