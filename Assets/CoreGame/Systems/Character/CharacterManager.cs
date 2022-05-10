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
        ATTACKING,
        INTERACTING,
        DYING
    }

    public CharacterAnimationEventsManager characterAnimationEventsManager;
    public CharacterCombatManager characterCombatManager;
    public CharacterInputManager characterInputManager;
    public CharacterInteractionManager characterInteractionManager;
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
        characterMovementManager.Link(this);
        characterStatsManager.Link(this);
        characterVisualsManager.Link(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        DisableMovement();
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
        SetCharacterState(CharacterState.ATTACKING);
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

    public void Kill()
    {
        characterVisualsManager.TriggerDeath();
        Move(Vector2.zero);
        characterCombatManager.SetInvincible(true);
        SetCharacterState(CharacterState.DYING);
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
