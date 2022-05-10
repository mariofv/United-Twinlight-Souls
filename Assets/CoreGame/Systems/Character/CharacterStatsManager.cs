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
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hurt(int damage)
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
