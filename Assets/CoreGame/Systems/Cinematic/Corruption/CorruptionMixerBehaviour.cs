using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CorruptionMixerBehaviour : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!Application.isPlaying)
		{
			return;
		}

		int inputCount = playable.GetInputCount();
		for (int i = 0; i < inputCount; ++i)
        {
			float inputWeight = playable.GetInputWeight(i);
			if (inputWeight > 0f)
			{
				ScriptPlayable<CorruptionBehaviour> inputPlayable = (ScriptPlayable<CorruptionBehaviour>)playable.GetInput(i);
				CorruptionBehaviour input = inputPlayable.GetBehaviour();

				Zone corruptedZone = input.corruptedZone;
				corruptedZone.SetZonePurification(inputWeight);

			}
		}
	}
}
