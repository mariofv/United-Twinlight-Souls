using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    private Cinematic currentCinematic;
    private bool isPlayingCinematic = false;

    public void PlayCinematic(Cinematic cinematic)
    {
        currentCinematic = cinematic;
        cinematic.onCinematicEnd.AddListener(EndCurrentCinematic);
        cinematic.Play();
        isPlayingCinematic = true;

        GameManager.instance.EnterGameState(GameManager.GameState.CINEMATIC);
    }
    
    public void EndCurrentCinematic()
    {
        if (!isPlayingCinematic)
        {
            return;
        }

        isPlayingCinematic = false;
        currentCinematic.onCinematicEnd.RemoveListener(EndCurrentCinematic);
        GameManager.instance.uiManager.cinematicUIManager.HideCinematicUI();
        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
    }

    public void SkipCinematic()
    {
        currentCinematic.Skip();
    }
}
