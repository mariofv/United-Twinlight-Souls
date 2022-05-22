using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CorruptionBehaviour : PlayableBehaviour
{
    public float startCorruption;
    public float endCorruption;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!Application.isPlaying)
        {
			return;
        }

		Zone corruptedZone = playerData as Zone;
		corruptedZone.SetZonePurification(info.weight);
	}
}
