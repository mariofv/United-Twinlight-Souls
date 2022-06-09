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

    public override void HideSpecialized(bool instant)
    {
        CloseCurrentTab();
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

    public override void OnNextTabPressed()
    {
        int nextTab = (currentTab + 1) % settingsTabs.Count;
        OpenTab(nextTab);
    }

    public override void OnPreviousTabPressed()
    {
        int nextTab = currentTab - 1;
        if (nextTab == -1)
        {
            nextTab = settingsTabs.Count - 1;
        }
        OpenTab(nextTab);
    }

    public void OpenTab(int tabIndex)
    {
        CloseCurrentTab();
        currentTab = tabIndex;
        settingsTabs[currentTab].Open();
    }

    private void CloseCurrentTab()
    {
        if (currentTab != -1)
        {
            settingsTabs[currentTab].Close();
        }
    }
}
