using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    private Cinematic currentCinematic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCinematic(Cinematic cinematic)
    {
        currentCinematic = cinematic;
        cinematic.Play();

        GameManager.instance.EnterGameState(GameManager.GameState.CINEMATIC);
    }
}
