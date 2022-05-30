using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ZoneCameraMixerBehaviour : PlayableBehaviour
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
				ScriptPlayable<ZoneCameraBehaviour> inputPlayable = (ScriptPlayable<ZoneCameraBehaviour>)playable.GetInput(i);
				ZoneCameraBehaviour input = inputPlayable.GetBehaviour();

				ZoneCamera zoneCamera = input.zoneCamera;
				float progress = Mathf.Lerp(input.startProgress, input.endProgress, inputWeight);
				zoneCamera.UpdateZoneCamera(progress);

			}
		}
	}
}
