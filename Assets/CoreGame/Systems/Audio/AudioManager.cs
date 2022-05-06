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

    public enum NPCSound
    {
        TALK,
        RELIEVE,
        TIP,
        THANK_YOU
    }

    [Header("NPC Sounds")]
    [SerializeField] private AudioSource npcAudioSource;
    [SerializeField] private NPCAudioClips npcAudioClips;
    private BalancedRandomSelector npcTalkSelector = new BalancedRandomSelector(3, 0.3f);

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

    public void PlayNPCSound(NPCSound npcSound)
    {
        switch (npcSound)
        {
            case NPCSound.TALK:
                int selected = npcTalkSelector.Select();
                if (selected == 0)
                {
                    npcAudioSource.PlayOneShot(npcAudioClips.talk1);
                }
                else if (selected == 1)
                {
                    npcAudioSource.PlayOneShot(npcAudioClips.talk2);
                }
                else if (selected == 2)
                {
                    npcAudioSource.PlayOneShot(npcAudioClips.talk3);
                }
                break;

            case NPCSound.RELIEVE:
                npcAudioSource.PlayOneShot(npcAudioClips.relieve);
                break;

            case NPCSound.TIP:
                npcAudioSource.PlayOneShotRandom(npcAudioClips.tip1, npcAudioClips.tip2, npcAudioClips.tip3);
                break;

            case NPCSound.THANK_YOU:
                npcAudioSource.PlayOneShot(npcAudioClips.thankYou);
                break;
        }
    }
}
