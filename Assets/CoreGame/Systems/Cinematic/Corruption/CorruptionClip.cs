using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CorruptionClip : PlayableAsset
{
	public float startCorruption;
	public float endCorruption;

	public ExposedReference<Zone> clipZone;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CorruptionBehaviour> playable = ScriptPlayable<CorruptionBehaviour>.Create(graph);
		CorruptionBehaviour clone = playable.GetBehaviour();

		clone.startCorruption = startCorruption;
		clone.endCorruption = endCorruption;
		clone.corruptedZone = clipZone.Resolve(graph.GetResolver());

		return playable;

	}
}