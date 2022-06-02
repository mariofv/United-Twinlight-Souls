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
        TimelineAsset timelineAsset = (TimelineAsset)cinematicDirector.playableAsset;

        CinemachineBrain brain = GameManager.instance.cameraManager.mainCamera.GetComponent<CinemachineBrain>();
        TrackAsset cinemachineTrack = timelineAsset.GetOutputTrack(1);
        cinematicDirector.SetGenericBinding(cinemachineTrack, brain);

        TrackAsset darkVeilTrack = timelineAsset.GetOutputTrack(2);
        cinematicDirector.SetGenericBinding(darkVeilTrack, GameManager.instance.uiManager.cinematicUIManager.cinematicDarkVeil);

        cinematicDirector.Play();
    }

    public void Skip()
    {
        cinematicDirector.time = cinematicDirector.playableAsset.duration;
    }

    private void EndCinematic(PlayableDirector director)
    {
        onCinematicEnd.Invoke();
    }
}
