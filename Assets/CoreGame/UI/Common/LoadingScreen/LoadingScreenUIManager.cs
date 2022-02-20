using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreenContainer;
    [SerializeField] private float displayTime;

    private bool showing = false;
    private bool hiding = false;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            currentTime += Time.unscaledTime;
            float progress = Mathf.Min(1f, currentTime / displayTime);
            loadingScreenContainer.alpha = progress;

            if (progress == 1f)
            {
                showing = false;
            }
        }

        if (hiding)
        {
            currentTime += Time.unscaledTime;
            float progress = Mathf.Min(1f, currentTime / displayTime);
            loadingScreenContainer.alpha = 1f- progress;

            if (progress == 1f)
            {
                hiding = false;
                loadingScreenContainer.gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        loadingScreenContainer.gameObject.SetActive(true);

        showing = true;
        hiding = false;

        currentTime = 0f;
    }

    public void Hide()
    {
        showing = false;
        hiding = true;

        currentTime = 0f;
    }

}
