using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public enum CharacterState
    {
        IDLE,
        MOVING,
        JUMPING,
        DASHING,
        LIGHT_ATTACKING,
        SPECIAL_ATTACKING,
        INTERACTING,
        STUNNED,
        DEAD
    }

    public CharacterAnimationEventsManager characterAnimationEventsManager;
    public CharacterCombatManager characterCombatManager;
    public CharacterInputManager characterInputManager;
    public CharacterInteractionManager characterInteractionManager;
    public CharacterLockManager characterLockManager;
    public CharacterMovementManager characterMovementManager;
    public CharacterStatsManager characterStatsManager;
    public CharacterVisualsManager characterVisualsManager;

    private CharacterState currentState;

    void Awake()
    {
        characterAnimationEventsManager.Link(this);
        characterCombatManager.Link(this);
        characterInputManager.Link(this);
        characterInteractionManager.Link(this);
        characterLockManager.Link(this);
        characterMovementManager.Link(this);
        characterStatsManager.Link(this);
        characterVisualsManager.Link(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        characterAnimationEventsManager.onDeathEnd.AddListener(OnDeathEnd);

        DisableMovement();
        DisableLock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector2 inputedMovement)
    {
        Vector2 normalizedInputedMovement = inputedMovement.normalized;

        characterMovementManager.SetInputedMovement(normalizedInputedMovement);
        characterVisualsManager.SetMoving(characterMovementManager.IsMoving());

        if (characterMovementManager.IsJumping())
        {
            SetCharacterState(CharacterState.JUMPING);
        }
        else if (characterMovementManager.IsMoving())
        {
            SetCharacterState(CharacterState.MOVING);
        }
        else
        {
            SetCharacterState(CharacterState.IDLE);
        }
    }

    public void Jump()
    {
        characterMovementManager.Jump();
    }

    public void LightAttack()
    {
        if (!characterCombatManager.CanExecuteLightAttack())
        {
            return;
        }

        characterCombatManager.LightAttack();
        SetCharacterState(CharacterState.LIGHT_ATTACKING);
    }

    public void SpecialAttack()
    {
        if (!characterCombatManager.CanExecuteSpecialAttack())
        {
            return;
        }

        characterCombatManager.SpecialAttack();
        SetCharacterState(CharacterState.SPECIAL_ATTACKING);
    }

    public void StartDash()
    {
        if (!characterMovementManager.CanDash())
        {
            return;
        }

        Vector2 lookingDirection = new Vector2(transform.forward.x, transform.forward.z);
        characterMovementManager.SetInputedMovement(lookingDirection, adjustToCamera:false);
        characterMovementManager.EnableDashMultiplier();

        characterCombatManager.SetInvincible(true);
        characterVisualsManager.TriggerStartDash();
        SetCharacterState(CharacterState.DASHING);
    }

    public void EndDash()
    {
        characterCombatManager.SetInvincible(false);
        characterVisualsManager.TriggerEndDash();
        Move(Vector2.zero);
    }

    public void RaiseShield()
    {
        characterCombatManager.RaiseShield();
    }

    public void ReleaseShield()
    {
        characterCombatManager.ReleaseShield();
    }

    public void Interact()
    {
        if (characterInteractionManager.CanInteract())
        {
            characterInteractionManager.Interact();
        }
    }

    public void Teleport(Vector3 position)
    {
        characterMovementManager.Teleport(position);
    }

    public void Orientate(Quaternion orientation)
    {
        characterMovementManager.Orientate(orientation);
    }

    public bool IsMovementEnabled()
    {
        return characterMovementManager.enabled;
    }

    public void EnableMovement()
    {
        characterMovementManager.enabled = true;
    }

    public void DisableMovement()
    {
        characterMovementManager.enabled = false;
    }

    public void EnableLock()
    {
        characterLockManager.enabled = true;
    }

    public void DisableLock()
    {
        characterLockManager.enabled = false;
    }

    public void StartStun()
    {
        characterVisualsManager.StartStun();
        characterStatsManager.StartStun();
        Move(Vector2.zero);
        characterCombatManager.EndLightAttackChain();
        SetCharacterState(CharacterState.STUNNED);
    }

    public void EndStun()
    {
        characterVisualsManager.EndStun();
        characterCombatManager.RepairShield();
        SetCharacterState(CharacterState.IDLE);
    }

    public void Kill()
    {
        characterVisualsManager.TriggerDeath();
        characterVisualsManager.ResetVisuals();
        
        Move(Vector2.zero);
        characterCombatManager.SetInvincible(true);
        characterCombatManager.OnCharacterDeath();

        SetCharacterState(CharacterState.DEAD);
    }

    public void OnDeathEnd()
    {
        GameManager.instance.levelManager.Respawn();
    }

    public bool IsDead()
    {
        return currentState == CharacterState.DEAD;
    }

    public void Revive()
    {
        characterVisualsManager.TriggerRevive();
        characterCombatManager.SetInvincible(false);
        characterStatsManager.SetFullHealth();
        SetCharacterState(CharacterState.IDLE);
    }

    public void ResetVisuals()
    {
        characterVisualsManager.ResetVisuals();
    }

    public void SetCharacterState(CharacterState characterState)
    {
        currentState = characterState;
    }

    public CharacterState GetCharacterState()
    {
        return currentState;
    }
}
