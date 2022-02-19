using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionUIManager : MonoBehaviour
{
    public float transitionDuration;

    [SerializeField]
    private Image blackScreen;

    private bool fadingIn = false;
    private bool fadingOut = false;

    private float currentFadingTime = 0f;

    void Awake()
    {
        blackScreen.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn)
        {
            currentFadingTime += Time.unscaledDeltaTime;
            float progress = Mathf.Min(currentFadingTime / transitionDuration, 1f);

            Color newColor = blackScreen.color;
            newColor.a = 1f - progress;
            blackScreen.color = newColor;

            if (progress == 1f)
            {
                fadingIn = false;
                blackScreen.gameObject.SetActive(false);
            }
        }
        else if (fadingOut)
        {
            currentFadingTime += Time.unscaledDeltaTime;
            float progress = Mathf.Min(currentFadingTime / transitionDuration, 1f);

            Color newColor = blackScreen.color;
            newColor.a = progress;
            blackScreen.color = newColor;

            if (progress == 1f)
            {
                fadingOut = false;
            }

        }
    }

    public void FadeIn()
    {
        blackScreen.gameObject.SetActive(true);
        fadingIn = true;
        fadingOut = false;
        currentFadingTime = 0f;
    }

    public bool IsFadingIn()
    {
        return fadingIn;
    }

    public void FadeOut()
    {
        blackScreen.gameObject.SetActive(true);
        fadingIn = false;
        fadingOut = true;
        currentFadingTime = 0f;
    }

    public bool IsFadingOut()
    {
        return fadingOut;
    }
}
