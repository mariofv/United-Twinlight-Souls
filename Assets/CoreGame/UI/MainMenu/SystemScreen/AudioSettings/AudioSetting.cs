using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string settingName;

    private const float MAX_DB = 0f;
    private const float MIN_DB = -40f;
    private const float MUTE_DB = -80f;
    private float step;

    private void Awake()
    {
        leftButton.onClick.AddListener(DecreaseSetting);
        rightButton.onClick.AddListener(IncreaseSetting);
        step = (MAX_DB - MIN_DB) / 10f;
    }

    public void IncreaseSetting()
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

    public void DecreaseSetting()
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

        float progress = ((newValue - MIN_DB) / (MAX_DB - MIN_DB)) * 10f;
        Debug.Log(progress);
        valueText.text = (Mathf.RoundToInt(progress).ToString());

    }
}
 