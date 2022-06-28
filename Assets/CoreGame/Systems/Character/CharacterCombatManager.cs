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

    [SerializeField] private SpecialAttack specialAttack;


    [SerializeField] private float orientatingTime;
    private bool isOrientatingPlayer = false;
    private Quaternion startingOrientation;
    private Quaternion targetOrientation;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterManager.characterAnimationEventsManager.onLightAttackEnd.AddListener(EndLightAttack);
        characterManager.characterAnimationEventsManager.onSpecialAttackEnd.AddListener(EndSpecialAttack);
    }

    private void Update()
    {
        if (isOrientatingPlayer)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(1f, currentTime / orientatingTime);

            transform.rotation = Quaternion.Lerp(startingOrientation, targetOrientation, progress);

            if (progress == 1f)
            {
                isOrientatingPlayer = false;
            }
        }
    }

    public void OnCharacterDeath()
    {
        isInLightAttackChain = false;
        currentLightAttackChain = 0;

        RepairShield();
    }

    public void SetInvincible(bool invincible)
    {
        playerHurtbox.enabled = !invincible;
    }

    public bool IsInvincible()
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.godMode)
            {
                return true;
            }
        }

        return !playerHurtbox.enabled;
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
            Vector3 lockedEnemyPosition = characterManager.characterLockManager.GetLockedEnemyHurtboxPosition();
            Vector3 lookDirection = lockedEnemyPosition - transform.position;
            lookDirection.y = 0f;

            startingOrientation = transform.rotation;
            targetOrientation = Quaternion.LookRotation(lookDirection);

            isOrientatingPlayer = true;
            currentTime = 0f;
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

    public void SpecialAttack()
    {
        characterManager.characterMovementManager.SetInputedMovement(Vector3.zero);
        characterManager.characterVisualsManager.TriggerSpecialAttack();

        if (characterManager.characterLockManager.IsLockingEnemy())
        {
            Vector3 lockedEnemyPosition = characterManager.characterLockManager.GetLockedEnemyHurtboxPosition();
            Vector3 lookDirection = lockedEnemyPosition - transform.position;
            lookDirection.y = 0f;

            startingOrientation = transform.rotation;
            targetOrientation = Quaternion.LookRotation(lookDirection);

            isOrientatingPlayer = true;
            currentTime = 0f;
        }

        //specialAttack
    }

    public bool CanExecuteSpecialAttack()
    {
        return true;
    }

    private void EndSpecialAttack()
    {
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
