using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class SystemScreenUIManager : MainMenuScreenUIManager
{
    [SerializeField] private List<SettingsTab> settingsTabs;
    private int currentTab = -1;

    public override void ShowSpecialized(bool instant)
    {
        uiElementCanvasGroup.gameObject.SetActive(true);
        OpenTab(0);
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd();
        hideTweens.Add(disableGameObjectAnimation);
    }

    public override void OnCancelPressed()
    {
        GameManager.instance.uiManager.mainMenuUIManager.OpenMainMenuScreen(MainMenuScreenId.LOGO);
    }

    private void NextTab()
    {
        int nextTab = (currentTab + 1) % settingsTabs.Count;
        OpenTab(nextTab);
    }

    private void PreviousTab()
    {
        int nextTab = (currentTab - 1) % settingsTabs.Count;
        OpenTab(nextTab);
    }

    private void OpenTab(int tabIndex)
    {
        if (currentTab != -1)
        {
            settingsTabs[currentTab].Close();
        }
        currentTab = tabIndex;
        settingsTabs[currentTab].Open();
    }
}
