using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Tutorial currentTutorial = null;

    public void StartTutorial(Tutorial tutorial)
    {
        currentTutorial = tutorial;
        currentTutorial.StartTutorial();
    }

    public void EndTutorial()
    {
        currentTutorial = null;
    }

    public Tutorial GetCurrentTutorial()
    {
        return currentTutorial;
    }
}
