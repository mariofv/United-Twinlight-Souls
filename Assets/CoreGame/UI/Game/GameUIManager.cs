using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public BossHealthBarUI bossHealthBarUI;
    public DamageIndicatorUI damageIndicatorUI;
    public DialogueUI dialogueUI;
    public HUDUI hudUI;
    public LockUI lockUI;
    public PauseUI pauseUI;
    public TutorialUI tutorialUI;

    public void HideAll()
    {
        HideCombatUI(instant: true);
    }

    public void ShowCombatUI(bool instant = false)
    {
        hudUI.Show(instant);
    }

    public void HideCombatUI(bool instant = false)
    {
        hudUI.Hide(instant);
        /*
        skillsUI.Hide(instant);
        */
    }

    public void ShowPauseUI(bool instant = false)
    {
        pauseUI.Show(instant);
    }

    public void HidePauseUI(bool instant = false)
    {
        pauseUI.Hide(instant);
    }
}
