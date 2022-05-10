using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : CharacterSubManager
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        SetFullHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFullHealth()
    {
        currentHealth = maxHealth;
    }

    public void Hurt(int damage)
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.godMode)
            {
                damage = 0;
            }
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);
        float currentHealthPercentage = (float)currentHealth / maxHealth;
        GameManager.instance.uiManager.gameUIManager.hudUI.SetHealth(currentHealthPercentage);

        if (currentHealth == 0)
        {
            characterManager.Kill();
        }
    }
}
