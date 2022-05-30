using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Image))]
[TrackClipType(typeof(DarkVeilClip))]
public class DarkVeilTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DarkVeilMixerBehaviour>.Create(graph, inputCount);
    }
}
