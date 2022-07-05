using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : CharacterSubManager
{
    private class LightAttackData
    {
        public LightAttackData(int damage, float minimumProgressToChain)
        {
            this.damage = damage;
            this.minimunProgressToChain = minimumProgressToChain;
        }

        public int damage;
        public float minimunProgressToChain;
    }

    [Header("Defense")]
    [SerializeField] private Collider playerHurtbox;
    [SerializeField] private Shield playerShield;

    [Header("Light attack")]
    [SerializeField] private LightAttackHitbox lightAttackHitbox;
    private List<LightAttackData> lightAttacks;
    private int currentLightAttackChain = -1;

    [Header("Special attack")]
    [SerializeField] private Transform specialAttackHolder;
    [SerializeField] private SpecialAttack specialAttack;
    [SerializeField] private float specialAttackCooldown;
    private float specialAttackCurrentTime = 0f;

    [Header("Targeting")]
    [SerializeField] private float orientatingTime;
    private bool isOrientatingPlayer = false;
    private Quaternion startingOrientation;
    private Quaternion targetOrientation;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterManager.characterAnimationEventsManager.onLightAttackEnd.AddListener(EndLightAttack);
        characterManager.characterAnimationEventsManager.onLightAttackEnableHitbox.AddListener(EnableLightAttackHitbox);
        characterManager.characterAnimationEventsManager.onLightAttackDisableHitbox.AddListener(DisableLightAttackHitbox);

        characterManager.characterAnimationEventsManager.onSpecialAttackThrow.AddListener(ThrowSpecialAttack);
        characterManager.characterAnimationEventsManager.onSpecialAttackEnd.AddListener(EndSpecialAttack);

        lightAttacks = new List<LightAttackData>();
        lightAttacks.Add(new LightAttackData(2, 0.5f));
        lightAttacks.Add(new LightAttackData(2, 0.5f));
        lightAttacks.Add(new LightAttackData(4, 0.5f));
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

        if (specialAttackCurrentTime > 0f)
        {
            specialAttackCurrentTime = Mathf.Max(0f, specialAttackCurrentTime - Time.deltaTime);
            float cooldownPercentage = 1f - specialAttackCurrentTime / specialAttackCooldown;
            GameManager.instance.uiManager.gameUIManager.hudUI.SetLight(cooldownPercentage);
        }
    }

    public void OnCharacterDeath()
    {
        currentLightAttackChain = -1;

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
            DisableLightAttackHitbox();
        }

        ++currentLightAttackChain;
        ExecuteLightAttack(currentLightAttackChain);
    }

    public bool CanExecuteLightAttack()
    {
        if (currentLightAttackChain == -1)
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
        characterManager.characterMovementManager.SetInputedMovement(Vector3.zero);
        characterManager.characterVisualsManager.TriggerLightAttack();

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

    private void EnableLightAttackHitbox()
    {
        lightAttackHitbox.gameObject.SetActive(true);
    }

    private void DisableLightAttackHitbox()
    {
        lightAttackHitbox.gameObject.SetActive(false);
    }
    
    private void EndLightAttack(int attackIndex)
    {
        //Debug.Log("End light attack recived index: " + attackIndex + " current: " + currentLightAttackChain);

        currentLightAttackChain = -1;
        characterManager.SetCharacterState(CharacterManager.CharacterState.IDLE);
    }

    public int GetCurrentLightAttackDamage()
    {
        if (currentLightAttackChain < 0 || currentLightAttackChain >= lightAttacks.Count)
        {
            lightAttackHitbox.gameObject.SetActive(false);
            return 0;
        }

        return lightAttacks[currentLightAttackChain].damage;
    }

    public void SpecialAttack()
    {
        characterManager.characterMovementManager.SetInputedMovement(Vector3.zero);
        SetInvincible(true);
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

        specialAttack.Cast(specialAttackHolder);
    }

    private void ThrowSpecialAttack()
    {
        if (characterManager.characterLockManager.IsLockingEnemy())
        {
            Vector3 lockedEnemyPosition = characterManager.characterLockManager.GetLockedEnemyHurtboxPosition();
            Vector3 lookDirection = lockedEnemyPosition - transform.position;
            lookDirection.y = 0f;

            specialAttack.Throw(lookDirection.normalized, characterManager.characterLockManager.GetLockedEnemyHurtbox());
        }
        else
        {
            specialAttack.Throw(transform.forward, null);
        }
        SetInvincible(false);
        specialAttackCurrentTime = specialAttackCooldown;
        GameManager.instance.uiManager.gameUIManager.hudUI.UseLight();
    }

    public bool CanExecuteSpecialAttack()
    {
        return specialAttack.IsAvailable() && specialAttackCurrentTime == 0f;
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
