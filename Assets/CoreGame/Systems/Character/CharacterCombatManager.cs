using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : CharacterSubManager
{
    [SerializeField] private List<LightAttack> lightAttacks;
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

        if (currentLightAttackChain >= 0)
        {
            EndCurrentLightAttack();
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

        if (currentLightAttackChain == lightAttacks.Count - 1)
        {
            return false;
        }

        float currentLightAttackAnimationProgress = characterManager.characterVisualsManager.GetCurrentAnimationProgress();

        return currentLightAttackAnimationProgress >= lightAttacks[currentLightAttackChain].minimunProgressToChain;
    }

    private void ExecuteLightAttack(int chainIndex)
    {
        if (!isInLightAttackChain)
        {
            isInLightAttackChain = true;
        }

        characterManager.characterVisualsManager.TriggerLightAttack();
        lightAttacks[currentLightAttackChain].gameObject.SetActive(true);
    }

    private void EndCurrentLightAttack()
    {
        if (currentLightAttackChain >= lightAttacks.Count || currentLightAttackChain < 0)
        {
            throw new UnityException("Trying to do light attack with an incorrect index (" + currentLightAttackChain + ")!");
        }

        lightAttacks[currentLightAttackChain].gameObject.SetActive(false);
    }
    
    private void EndLightAttack()
    {
        if (!isInLightAttackChain)
        {
            return;
        }

        EndCurrentLightAttack();
        isInLightAttackChain = false;
        currentLightAttackChain = -1;
    }
}
