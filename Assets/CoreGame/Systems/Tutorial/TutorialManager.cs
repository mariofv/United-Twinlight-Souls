using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Tutorial currentTutorial = null;

    public void StartTutorial(Tutorial tutorial)
    {
        currentTutorial = tutorial;
        string text = currentTutorial.GetText(GameManager.instance.inputManager.GetInputDeviceType());

        GameManager.instance.uiManager.gameUIManager.tutorialUI.SetTutorialText(text);
        GameManager.instance.uiManager.gameUIManager.tutorialUI.Show();
    }

    public void EndTutorial()
    {
        currentTutorial = null;
        GameManager.instance.uiManager.gameUIManager.tutorialUI.Hide();
    }

    public void OnInputDeviceChanged(InputManager.InputDeviceType inputDeviceType)
    {
        if (currentTutorial == null)
        {
            return;
        }

        string text = currentTutorial.GetText(inputDeviceType);

        GameManager.instance.uiManager.gameUIManager.tutorialUI.SetTutorialText(text);
    }
}
