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
        if (index == currentLevelDescription)
        {
            return;
        }

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

    public override void OnCancelPressed()
    {
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.SELECT_CHARACTER);
    }

    public override void OnConfirmPressed()
    {
        GameManager.instance.InitGame(currentLevelDescription);
    }

    public override void OnLeftPressed()
    {
        int nextIndex = currentLevelDescription - 1;
        if (nextIndex == -1)
        {
            nextIndex = GameManager.instance.progressionManager.GetMaxLevelUnlocked();
        }
        OpenLevelDescription(nextIndex);
    }

    public override void OnRightPressed()
    {
        int nextIndex = (currentLevelDescription + 1) % (GameManager.instance.progressionManager.GetMaxLevelUnlocked() + 1);
        OpenLevelDescription(nextIndex);
    }
}
