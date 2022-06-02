using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Zone))]
[TrackClipType(typeof(CorruptionClip))]
public class CorruptionTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<CorruptionMixerBehaviour>.Create(graph, inputCount);
    }
}
