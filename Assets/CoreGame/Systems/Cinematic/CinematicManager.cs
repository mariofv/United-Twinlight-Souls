using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCinematic(PlayableAsset cinematic)
    {
        playableDirector.playableAsset = cinematic;
        playableDirector.Play();
    }
}
