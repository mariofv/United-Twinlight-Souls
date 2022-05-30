using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DarkVeilClip : PlayableAsset
{
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<DarkVeilBehaviour> playable = ScriptPlayable<DarkVeilBehaviour>.Create(graph);
		return playable;
	}
}
