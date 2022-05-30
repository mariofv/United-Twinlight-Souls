using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DarkVeilMixerBehaviour : PlayableBehaviour
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
				ScriptPlayable<DarkVeilBehaviour> inputPlayable = (ScriptPlayable<DarkVeilBehaviour>)playable.GetInput(i);
				DarkVeilBehaviour input = inputPlayable.GetBehaviour();
				Image darkVeil = playerData as Image;
				darkVeil.SetAlpha(inputWeight);
			}
		}
	}
}
