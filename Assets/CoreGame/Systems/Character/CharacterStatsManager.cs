using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : CharacterSubManager
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float stunnedTime;

    private float currentTime = 0f;

    private int currentHealth;

    private void Start()
    {
        characterManager.characterCombatManager.SetShieldRecoveryTime(stunnedTime);
    }

    private void Update()
    {
        if (characterManager.GetCharacterState() == CharacterManager.CharacterState.STUNNED)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= stunnedTime)
            {
                characterManager.EndStun();
            }
        }
    }

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

    public void StartStun()
    {
        currentTime = 0f;
    }
}
