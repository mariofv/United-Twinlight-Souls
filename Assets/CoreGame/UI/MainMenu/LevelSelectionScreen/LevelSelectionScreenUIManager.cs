using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class LevelSelectionScreenUIManager : MainMenuScreenUIManager
{
    [SerializeField] private List<LevelDescriptionScreen> levelDescriptionScreens;
    private int currentLevelDescription = -1;
    
    public override void ShowSpecialized(bool instant)
    {
        uiElementCanvasGroup.gameObject.SetActive(true);
        OpenLevelDescription(0);
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd().Unscaled();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd().Unscaled();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd().Unscaled();
        hideTweens.Add(disableGameObjectAnimation);
    }

    private void OpenLevelDescription(int index)
    {
        CloseCurrentLevelDescription();
        currentLevelDescription = index;
        levelDescriptionScreens[currentLevelDescription].Open();
    }

    private void CloseCurrentLevelDescription()
    {
        if (currentLevelDescription == -1)
        { 
            return;
        }
        levelDescriptionScreens[currentLevelDescription].Close();
    }
}
