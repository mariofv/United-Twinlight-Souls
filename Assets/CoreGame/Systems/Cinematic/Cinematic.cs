using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

public class Cinematic : MonoBehaviour
{
    public UnityEvent onCinematicEnd;
    [SerializeField] private PlayableDirector cinematicDirector;

    private void Awake()
    {
        cinematicDirector.stopped += EndCinematic;
    }

    public void Play()
    {
        CinemachineBrain brain = GameManager.instance.cameraManager.mainCamera.GetComponent<CinemachineBrain>();
        TimelineAsset timelineAsset = (TimelineAsset)cinematicDirector.playableAsset;
        TrackAsset track = timelineAsset.GetOutputTrack(1);
        cinematicDirector.SetGenericBinding(track, brain);

        cinematicDirector.Play();
    }

    private void EndCinematic(PlayableDirector director)
    {
        onCinematicEnd.Invoke();
    }
}
