using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeReference] public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();

    [System.Serializable]
    public abstract class TutorialEvent
    {
        public UnityEvent<TutorialEvent> onTutorialEventEnd;

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
        }

        protected override void EndEventSpecialized()
        {
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
