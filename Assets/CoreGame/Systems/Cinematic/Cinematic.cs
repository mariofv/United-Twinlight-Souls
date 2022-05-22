using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private PlayableDirector cinematicDirector;

    public void Play()
    {
        cinematicDirector.Play();
    }
}
