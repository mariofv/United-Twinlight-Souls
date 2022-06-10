using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class AudioSetting : Setting
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string settingName;

    [SerializeField] private Image barFill;
    [SerializeField] private TextMeshProUGUI valueText;

    private const float MAX_DB = 0f;
    private const float MIN_DB = -40f;
    private const float MUTE_DB = -80f;
    private float step;

    protected override void Awake()
    {
        base.Awake();
        step = (MAX_DB - MIN_DB) / 10f;
    }

    public override void IncreaseSetting()
    {
        float currentValue;
        audioMixer.GetFloat(settingName, out currentValue);
        if (currentValue == MUTE_DB)
        {
            currentValue = MIN_DB;
        }

        currentValue = Mathf.Min(currentValue + step, MAX_DB);
        SetAudioSettingVolume(currentValue);
    }

    public override void DecreaseSetting()
    {
        float currentValue;
        audioMixer.GetFloat(settingName, out currentValue);

        currentValue = Mathf.Max(currentValue - step, MIN_DB);
        SetAudioSettingVolume(currentValue);
    }

    private void SetAudioSettingVolume(float newValue)
    {
        if (newValue == MIN_DB)
        {
            audioMixer.SetFloat(settingName, MUTE_DB);
        }
        else
        {
            audioMixer.SetFloat(settingName, newValue);
        }

        float progress = (newValue - MIN_DB) / (MAX_DB - MIN_DB);
        barFill.fillAmount = progress;

        valueText.text = (Mathf.RoundToInt(progress * 10f).ToString());

    }
}
 