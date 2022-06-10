using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class GameplaySetting : Setting
{
    private enum GameplaySettingType
    {
        DAMAGE_INDICATOR,
        VIBRATION,
        SCREEN_MODE
    }

    [SerializeField] private GameplaySettingType settingType;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private string onValue;
    [SerializeField] private string offValue;

    protected override void Awake()
    {
        base.Awake();
        SetDisplayedValue();
    }

    public override void IncreaseSetting()
    {
        SwitchValue();
    }

    public override void DecreaseSetting()
    {
        SwitchValue();
    }

    private void SwitchValue()
    {
        switch (settingType)
        {
            case GameplaySettingType.DAMAGE_INDICATOR:
                GameManager.instance.settingsManager.damageIndicatorEnabled = !GameManager.instance.settingsManager.damageIndicatorEnabled;
                break;

            case GameplaySettingType.VIBRATION:
                GameManager.instance.settingsManager.vibrationEnabled = !GameManager.instance.settingsManager.vibrationEnabled;
                break;

            case GameplaySettingType.SCREEN_MODE:
                GameManager.instance.settingsManager.fullscreenEnabled = !GameManager.instance.settingsManager.fullscreenEnabled;
                Screen.fullScreenMode = (FullScreenMode)(GameManager.instance.settingsManager.fullscreenEnabled ? 0 : 3);
                break;
        }
        SetDisplayedValue();
    }

    private void SetDisplayedValue()
    {
        bool onValueActivated = false;

        switch (settingType)
        {
            case GameplaySettingType.DAMAGE_INDICATOR:
                onValueActivated = GameManager.instance.settingsManager.damageIndicatorEnabled;
                break;
            case GameplaySettingType.VIBRATION:
                onValueActivated = GameManager.instance.settingsManager.vibrationEnabled;
                break;
            case GameplaySettingType.SCREEN_MODE:
                onValueActivated = GameManager.instance.settingsManager.fullscreenEnabled;
                break;
        }

        valueText.text = onValueActivated ? onValue : offValue;
    }
}
 