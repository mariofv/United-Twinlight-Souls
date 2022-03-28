using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource levelMusicSource;
    [SerializeField] private AudioClip mainMenuMusic;

    public enum UISound
    {
        CLICK,
        SELECT
    }

    [Header("UI Sounds")]
    [SerializeField] private AudioSource uiClickSound;
    [SerializeField] private AudioSource uiSelectSound;

    public void PlayMainMenuMusic()
    {
        SetCurrentLevelMusic(mainMenuMusic);
    }

    public void SetCurrentLevelMusic(AudioClip levelMusicClip)
    {
        levelMusicSource.clip = levelMusicClip;
        levelMusicSource.Play();
    }

    public void PlayUISound(UISound uiSound)
    {
        switch (uiSound)
        {
            case UISound.CLICK:
                uiClickSound.Play();
                break;

            case UISound.SELECT:
                uiSelectSound.Play();
                break;
        }
    }
}
