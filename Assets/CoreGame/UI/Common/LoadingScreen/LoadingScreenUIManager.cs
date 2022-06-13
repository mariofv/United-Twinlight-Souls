using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweening;

public class LoadingScreenUIManager : UIElement
{
    [SerializeField] private List<Image> loadingScreenTips;

    private float currentTime = 0;
    private int currentTip = 0;
    private List<TweeningAnimation> tipsShowTweens;
    private List<TweeningAnimation> tipsHideTweens;

    private bool displayingTips = false;

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).Unscaled().DontKillOnEnd();
        showTweens.Add(fadeAnimation);

        tipsShowTweens = new List<TweeningAnimation>();
        for (int i = 0; i < loadingScreenTips.Count; ++i)
        {
            TweeningAnimation tipFadeAnimation = loadingScreenTips[i].TweenFade(UISettings.GameUISettings.DISPLAY_TIME * 4, 0f, 1f).Unscaled().DontKillOnEnd();
            tipsShowTweens.Add(tipFadeAnimation);
        }
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).Unscaled().DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).Unscaled().DontKillOnEnd();
        hideTweens.Add(disableAnimation);

        tipsHideTweens = new List<TweeningAnimation>();
        for (int i = 0; i < loadingScreenTips.Count; ++i)
        {
            TweeningAnimation tipFadeAnimation = loadingScreenTips[i].TweenFade(UISettings.GameUISettings.DISPLAY_TIME * 4, 1f, 0f).Unscaled().DontKillOnEnd();
            tipsHideTweens.Add(tipFadeAnimation);

            TweeningAnimation tipDisableAnimation = loadingScreenTips[i].gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME * 4).Unscaled().DontKillOnEnd();
            tipsHideTweens.Add(tipDisableAnimation);
        }
    }

    public override void ShowSpecialized(bool instant)
    {
        currentTime = 0;
        currentTip = 0;
        //displayingTips = true;

        /*
        loadingScreenTips[currentTip].gameObject.SetActive(true);
        tipsShowTweens[currentTip].Play();
        */
    }

    public override void HideSpecialized(bool instant)
    {
        for (int i = 0; i < loadingScreenTips.Count; ++i)
        {
            loadingScreenTips[i].gameObject.SetActive(false);
        }
        displayingTips = false;
    }

    private void Update()
    {
        if (!displayingTips)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= UISettings.GameUISettings.LOADING_SCREEN_TIP_DISPLAY_TIME)
        {
            ShowNextTip();
        }
    }

    public void ShowNextTip()
    {
        int lastTip = currentTip;

        ++currentTip;
        if (currentTip == loadingScreenTips.Count)
        {
            currentTip = 0;
        }

        ShowTip(lastTip, currentTip);
    }

    public void ShowPreviousTip()
    {
        int lastTip = currentTip;

        --currentTip;
        if (currentTip == -1)
        {
            currentTip = loadingScreenTips.Count - 1;
        }

        ShowTip(lastTip, currentTip);
    }

    private void ShowTip(int lastTip, int currentTip)
    {
        return;

        tipsHideTweens[lastTip * 2].Rewind();
        tipsHideTweens[lastTip * 2].Play();
        tipsHideTweens[lastTip * 2 + 1].Rewind();
        tipsHideTweens[lastTip * 2 + 1].Play();

        loadingScreenTips[currentTip].gameObject.SetActive(true);
        tipsShowTweens[currentTip].Rewind();
        tipsShowTweens[currentTip].Play();

        currentTime = 0f;
    }
}
