using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Tutorial currentTutorial = null;
    private int currentTutorialEvent;

    public void StartTutorial(Tutorial tutorial)
    {
        currentTutorial = tutorial;
        currentTutorialEvent = 0;

        StartTutorialEvent(currentTutorial.tutorialEvents[currentTutorialEvent]);
    }

    public void EndTutorial()
    {
        currentTutorial = null;
    }

    private void StartTutorialEvent(Tutorial.TutorialEvent tutorialEvent)
    {
        tutorialEvent.StartEvent();
        tutorialEvent.onTutorialEventEnd.AddListener(OnTutorialEventEnd);
    }

    private void EndCurrentTutorialEvent()
    {
        currentTutorial.tutorialEvents[currentTutorialEvent].EndEvent();
    }

    private void OnTutorialEventEnd(Tutorial.TutorialEvent tutorialEvent)
    {
        tutorialEvent.onTutorialEventEnd.RemoveListener(OnTutorialEventEnd);
        
        ++currentTutorialEvent;
        if (currentTutorialEvent == currentTutorial.tutorialEvents.Count)
        {
            EndTutorial();
        }
        else
        {
            StartTutorialEvent(currentTutorial.tutorialEvents[currentTutorialEvent]);
        }
    }

    public void OnInputDeviceChanged(InputManager.InputDeviceType inputDeviceType)
    {
        if (currentTutorial == null)
        {
            return;
        }

        Tutorial.ShowTextTutorialEvent currentEvent = currentTutorial.tutorialEvents[currentTutorialEvent] as Tutorial.ShowTextTutorialEvent;
        if (currentEvent != null)
        {
            currentEvent.OnInputDeviceChanged(inputDeviceType);
        }
    }
}
