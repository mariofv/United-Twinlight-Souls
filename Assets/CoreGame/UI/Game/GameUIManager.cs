using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public DamageIndicatorUI damageIndicatorUI;
    public DialogueUI dialogueUI;
    public PauseUI pauseUI;
    public HUDUI hudUI;
    public BossHealthBarUI bossHealthBarUI;

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
