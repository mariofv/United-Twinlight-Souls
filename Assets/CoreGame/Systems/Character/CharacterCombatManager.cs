using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : CharacterSubManager
{
    [SerializeField] private Collider playerHurtbox;
    [SerializeField] private Shield playerShield;
    [SerializeField] private List<LightAttack> lightAttacks;
    private bool isInLightAttackChain = false;
    private int currentLightAttackChain = -1;

    private Enemy currentLockedEnemy = null;

    // Start is called before the first frame update
    void Start()
    {
        characterManager.characterAnimationEventsManager.onLightAttackEnd.AddListener(EndLightAttack);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInvincible(bool invincible)
    {
        playerHurtbox.enabled = !invincible;
    }

    public void LightAttack()
    {
        if (currentLightAttackChain >= 0)
        {
            EndCurrentLightAttack();
        }

        ++currentLightAttackChain;
        ExecuteLightAttack(currentLightAttackChain);
    }

    public bool CanExecuteLightAttack()
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

        characterManager.characterMovementManager.SetInputedMovement(Vector3.zero);
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
        characterManager.SetCharacterState(CharacterManager.CharacterState.IDLE);
    }

    public void RaiseShield()
    {
        if (!playerShield.IsBroken())
        {
            playerShield.Raise();
            SetInvincible(true);
        }
    }

    public void ReleaseShield()
    {
        if (!playerShield.IsBroken())
        {
            playerShield.Release();
            SetInvincible(false);
        }
    }

    public void RepairShield()
    {
        if (playerShield.IsBroken())
        {
            playerShield.RepairShield();
        }
    }

    public void HitShield(int damage, Vector3 hitPosition)
    {
        playerShield.HitShield(damage, hitPosition);
    }

    public bool IsShieldActive()
    {
        return playerShield.IsRaised();
    }

    public void SwitchLockEnemy()
    {
        if (currentLockedEnemy == null)
        {
            LockClosestEnemy();
        }
        else
        {
            UnlockEnemy();
        }
    }

    public void ChangeLockedEnemy()
    {
        if (currentLockedEnemy == null)
        {
            return;
        }

        Enemy nextEnemy = GameManager.instance.enemyManager.GetNextEnemy(currentLockedEnemy);
        if (nextEnemy != null)
        {
            UnlockEnemy();
            LockEnemy(nextEnemy);
        }
    }

    private void LockClosestEnemy()
    {
        Enemy closestEnemy = GameManager.instance.enemyManager.GetClosestEnemy(transform.position);
        if (closestEnemy == null)
        {
            return;
        }

        LockEnemy(closestEnemy);
    }

    private void LockEnemy(Enemy enemy)
    {
        currentLockedEnemy = enemy;
        currentLockedEnemy.onSpawnedEnemyDead.AddListener(OnLockedEnemyDeath);

        GameManager.instance.uiManager.gameUIManager.lockCursorUI.LockOnEnemy(currentLockedEnemy.transform);
    }

    private void UnlockEnemy()
    {
        currentLockedEnemy.onSpawnedEnemyDead.RemoveListener(OnLockedEnemyDeath);
        currentLockedEnemy = null;

        GameManager.instance.uiManager.gameUIManager.lockCursorUI.UnlockEnemy();
    }

    private void OnLockedEnemyDeath()
    {
        UnlockEnemy();
    }
}
