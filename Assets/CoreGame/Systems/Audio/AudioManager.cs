using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource levelMusicSource;

    public enum UISound
    {
        CLICK
    }

    [Header("UI Sounds")]
    [SerializeField] private AudioSource uiClickSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
