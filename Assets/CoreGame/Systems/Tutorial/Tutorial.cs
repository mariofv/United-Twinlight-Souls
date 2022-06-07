using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeReference] public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();
    private int currentTutorialEvent;

    public void StartTutorial()
    {
        currentTutorialEvent = 0;

        StartTutorialEvent(tutorialEvents[currentTutorialEvent]);
    }

    private void StartTutorialEvent(TutorialEvent tutorialEvent)
    {
        tutorialEvent.StartEvent();
        tutorialEvent.onTutorialEventEnd.AddListener(OnTutorialEventEnd);
    }

    private void OnTutorialEventEnd(TutorialEvent tutorialEvent)
    {
        tutorialEvent.onTutorialEventEnd.RemoveListener(OnTutorialEventEnd);

        ++currentTutorialEvent;
        if (currentTutorialEvent == tutorialEvents.Count)
        {
            GameManager.instance.tutorialManager.EndTutorial();
        }
        else
        {
            StartTutorialEvent(tutorialEvents[currentTutorialEvent]);
        }
    }
    public void OnInputDeviceChanged(InputManager.InputDeviceType inputDeviceType)
    {
        ShowTextTutorialEvent currentEvent = tutorialEvents[currentTutorialEvent] as ShowTextTutorialEvent;
        if (currentEvent != null)
        {
            currentEvent.OnInputDeviceChanged(inputDeviceType);
        }
    }

    public void AnyKeyPressed()
    {
        ShowTextTutorialEvent currentEvent = tutorialEvents[currentTutorialEvent] as ShowTextTutorialEvent;
        if (currentEvent != null)
        {
            currentEvent.EndEvent();
        }
    }

    [System.Serializable]
    public abstract class TutorialEvent
    {
        [HideInInspector] public UnityEvent<TutorialEvent> onTutorialEventEnd;

        public abstract void StartEvent();
        protected virtual void EndEventSpecialized() {}
        public void EndEvent()
        {
            EndEventSpecialized();
            onTutorialEventEnd.Invoke(this);
        }
    }

    public class SpawnEnemyTutorialEvent : TutorialEvent
    {
        [SerializeField] private EnemyWave enemyWave;
        public override void StartEvent()
        {
            enemyWave.onWaveSpawnEnd.AddListener(EndEvent);
            enemyWave.StartWave();
        }

        protected override void EndEventSpecialized()
        {
            enemyWave.onWaveSpawnEnd.RemoveListener(EndEvent);
        }
    }

    public class ShowTextTutorialEvent : TutorialEvent
    {
        [SerializeField] private string pcText;
        [SerializeField] private string psText;
        [SerializeField] private string xboxText;

        public override void StartEvent()
        {
            string text = GetText(GameManager.instance.inputManager.GetInputDeviceType());

            GameManager.instance.uiManager.gameUIManager.tutorialUI.SetTutorialText(text);
            GameManager.instance.uiManager.gameUIManager.tutorialUI.Show();

            GameManager.instance.EnterGameState(GameManager.GameState.TUTORIAL);
            Time.timeScale = 0;
        }

        protected override void EndEventSpecialized()
        {
            GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
            Time.timeScale = 1f;
            GameManager.instance.uiManager.gameUIManager.tutorialUI.Hide();
        }

        public void OnInputDeviceChanged(InputManager.InputDeviceType inputDeviceType)
        {
            string text = GetText(inputDeviceType);

            GameManager.instance.uiManager.gameUIManager.tutorialUI.SetTutorialText(text);
        }

        public string GetText(InputManager.InputDeviceType inputDeviceType)
        {
            switch (inputDeviceType)
            {
                case InputManager.InputDeviceType.KEYBOARD:
                    return pcText;

                case InputManager.InputDeviceType.PS5_CONTROLLER:
                    return psText;

                case InputManager.InputDeviceType.XBOX_CONTROLLER:
                    return xboxText;

                default:
                    return "";
            }
        }
    }
}
