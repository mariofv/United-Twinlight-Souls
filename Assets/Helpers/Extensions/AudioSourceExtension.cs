using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtension
{
    public static void PlayOneShotRandom(this AudioSource audioSource, params AudioClip[] clips)
    {
        int numberOfAudioClips = clips.Length;
        int randomAudioClip = Random.Range(0, numberOfAudioClips);
        audioSource.PlayOneShot(clips[randomAudioClip]);
    }
}
