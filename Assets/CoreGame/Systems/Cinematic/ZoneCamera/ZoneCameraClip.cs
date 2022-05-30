using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ZoneCameraClip : PlayableAsset
{
	public float startProgress;
	public float endProgress;

	public ExposedReference<ZoneCamera> clipZoneCamera;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<ZoneCameraBehaviour> playable = ScriptPlayable<ZoneCameraBehaviour>.Create(graph);
		ZoneCameraBehaviour clone = playable.GetBehaviour();

		clone.startProgress = startProgress;
		clone.endProgress = endProgress;
		clone.zoneCamera = clipZoneCamera.Resolve(graph.GetResolver());

		return playable;

	}
}
