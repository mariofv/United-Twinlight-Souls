using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : CharacterSubManager
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    public void SetFullHealth()
    {
        currentHealth = maxHealth;
        GameManager.instance.uiManager.gameUIManager.hudUI.SetHealth(1f);
    }

    public void Heal(int amountHealed)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amountHealed);
        float currentHealthPercentage = (float)currentHealth / maxHealth;
        GameManager.instance.uiManager.gameUIManager.hudUI.SetHealth(currentHealthPercentage);
    }

    public void Hurt(int damage, Vector3 hitPosition)
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.godMode)
            {
                damage = 0;
            }
        }

        if (characterManager.characterCombatManager.IsShieldActive())
        {
            characterManager.characterCombatManager.HitShield(damage, hitPosition);
        }
        else
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            float currentHealthPercentage = (float)currentHealth / maxHealth;
            GameManager.instance.uiManager.gameUIManager.hudUI.SetHealth(currentHealthPercentage);

            if (currentHealth == 0)
            {
                characterManager.Kill();
            }
        }
    }
}
