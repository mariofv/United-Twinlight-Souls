using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    private Cinematic currentCinematic;

    public void PlayCinematic(Cinematic cinematic)
    {
        currentCinematic = cinematic;
        cinematic.onCinematicEnd.AddListener(EndCurrentCinematic);
        cinematic.Play();

        GameManager.instance.EnterGameState(GameManager.GameState.CINEMATIC);
    }
    
    public void EndCurrentCinematic()
    {
        currentCinematic.onCinematicEnd.RemoveListener(EndCurrentCinematic);
        GameManager.instance.uiManager.cinematicUIManager.HideCinematicUI();
        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
    }

    public void SkipCinematic()
    {
        currentCinematic.Skip();
    }
}
