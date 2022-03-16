using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : CharacterSubManager
{
    private bool isInLightAttackChain = false;
    private int currentLightAttackChain = -1;

    // Start is called before the first frame update
    void Start()
    {
        characterManager.characterAnimationEventsManager.onLightAttackEnd.AddListener(EndLightAttack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightAttack()
    {
        if (!CanExecuteLightAttack())
        {
            return;
        }
        ++currentLightAttackChain;
        ExecuteLightAttack(currentLightAttackChain);
    }

    private bool CanExecuteLightAttack()
    {
        if (!isInLightAttackChain)
        {
            return true;
        }

        return false;
    }

    private void ExecuteLightAttack(int chainIndex)
    {
        if (!isInLightAttackChain)
        {
            isInLightAttackChain = true;
        }
        characterManager.characterVisualsManager.TriggerLightAttack();
    }

    private void EndLightAttack()
    {
        isInLightAttackChain = false;
        currentLightAttackChain = -1;
    }
}
