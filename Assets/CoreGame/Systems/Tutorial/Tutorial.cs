using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [HideInInspector] public UnityEvent onTutorialEnd;
    public ProgressionManager.Progression associatedProgression;
    [SerializeReference] public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();
    private int currentTutorialEvent = -1;

    private void Update()
    {
        if (currentTutorialEvent < 0 || currentTutorialEvent >= tutorialEvents.Count)
        { 
            return;
        }

        tutorialEvents[currentTutorialEvent].Update(Time.unscaledDeltaTime);
    }

    public void StartTutorial()
    {
        currentTutorialEvent = 0;

        StartTutorialEvent(tutorialEvents[currentTutorialEvent]);
    }

    public void EndTutorial()
    {
        currentTutorialEvent = -1;
        GameManager.instance.tutorialManager.EndTutorial();
        onTutorialEnd.Invoke();
    }

    private void StartTutorialEvent(TutorialEvent tutorialEvent)
    {
        tutorialEvent.StartEvent(this);
        tutorialEvent.onTutorialEventEnd.AddListener(OnTutorialEventEnd);
    }

    private void OnTutorialEventEnd(TutorialEvent tutorialEvent)
    {
        tutorialEvent.onTutorialEventEnd.RemoveListener(OnTutorialEventEnd);

        ++currentTutorialEvent;
        if (currentTutorialEvent != tutorialEvents.Count)
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
            if (currentEvent.CanBeInteracted())
            {
                currentEvent.EndEvent();
            }
        }
    }

    [System.Serializable]
    public abstract class TutorialEvent
    {
        [HideInInspector] public UnityEvent<TutorialEvent> onTutorialEventEnd;

        public virtual void Update(float deltaTime) { }

        public abstract void StartEvent(Tutorial tutorialOwner);
        protected virtual void EndEventSpecialized() {}
        public void EndEvent()
        {
            EndEventSpecialized();
            onTutorialEventEnd.Invoke(this);
        }
    }

    public class StartCombatAreaTutorialEvent : TutorialEvent
    {
        [SerializeField] private EnemyCombatArea enemyCombatArea;
        public override void StartEvent(Tutorial tutorialOwner)
        {
            enemyCombatArea.onCombatAreaEnd.AddListener(EndEvent);
            enemyCombatArea.StartCombatArea();
            enemyCombatArea.combatAreaPlayerDetectionCollider.enabled = true;
        }

        protected override void EndEventSpecialized()
        {
            enemyCombatArea.onCombatAreaEnd.RemoveListener(EndEvent);
        }
    }

    public class WaitTimeTutorialEvent : TutorialEvent
    {
        [SerializeField] private float timeToWait;
        private float currentTime = 0f;

        public override void StartEvent(Tutorial tutorialOwner)
        {
            currentTime = 0f;
        }

        public override void Update(float deltaTime)
        {
            currentTime += deltaTime;
            if (currentTime > timeToWait)
            {
                EndEvent();
            }
        }
    }

    public class ShowTextTutorialEvent : TutorialEvent
    {
        [SerializeField] private string pcText;
        [SerializeField] private string psText;
        [SerializeField] private string xboxText;

        private const float blockedInteractionTime = 1f;
        private float currentTime = 0f;

        public override void Update(float deltaTime)
        {
            currentTime += deltaTime;
        }

        public override void StartEvent(Tutorial tutorialOwner)
        {
            string text = GetText(GameManager.instance.inputManager.GetInputDeviceType());

            GameManager.instance.progressionManager.UnlockProgression(tutorialOwner.associatedProgression);

            GameManager.instance.uiManager.gameUIManager.tutorialUI.SetTutorialText(text);
            GameManager.instance.uiManager.gameUIManager.tutorialUI.Show();

            GameManager.instance.EnterGameState(GameManager.GameState.TUTORIAL);
            Time.timeScale = 0; 
            currentTime = 0f;
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

        public bool CanBeInteracted()
        {
            return currentTime >= blockedInteractionTime;
        }

        private string GetText(InputManager.InputDeviceType inputDeviceType)
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
