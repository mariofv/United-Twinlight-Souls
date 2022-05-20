using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : CharacterSubManager
{
    [SerializeField] private Collider playerHurtbox;
    [SerializeField] private Shield playerShield;
    [SerializeField] private List<LightAttack> lightAttacks;
    [SerializeField] private float maxRotationAngleTowardsLockedEnemy;
    private bool isInLightAttackChain = false;
    private int currentLightAttackChain = -1;

    // Start is called before the first frame update
    void Start()
    {
        characterManager.characterAnimationEventsManager.onLightAttackEnd.AddListener(EndLightAttack);
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

        if (characterManager.characterLockManager.IsLockingEnemy())
        {
            Vector3 lockedEnemyPosition = characterManager.characterLockManager.GetLockedEnemyPosition();

            Vector3 lookDirection = lockedEnemyPosition - transform.position;
            lookDirection.y = 0f;

            float angleTowardsEnemy = Vector3.SignedAngle(transform.forward, lookDirection, Vector3.up);
            angleTowardsEnemy = Mathf.Clamp(angleTowardsEnemy, -maxRotationAngleTowardsLockedEnemy, maxRotationAngleTowardsLockedEnemy);
            transform.rotation = Quaternion.AngleAxis(angleTowardsEnemy, Vector3.up) * transform.rotation;
        }
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
}
