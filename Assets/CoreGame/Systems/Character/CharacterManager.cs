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
        INTERACTING
    }

    public CharacterAnimationEventsManager characterAnimationEventsManager;
    public CharacterCombatManager characterCombatManager;
    public CharacterInputManager characterInputManager;
    public CharacterMovementManager characterMovementManager;
    public CharacterStatsManager characterStatsManager;
    public CharacterVisualsManager characterVisualsManager;

    private CharacterState currentState;

    void Awake()
    {
        characterAnimationEventsManager.Link(this);
        characterCombatManager.Link(this);
        characterInputManager.Link(this);
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

    public void Teleport(Vector3 position)
    {
        characterMovementManager.Teleport(position);
    }

    public void Orientate(Quaternion orientation)
    {
        characterMovementManager.Orientate(orientation);
    }

    public void EnableMovement()
    {
        characterMovementManager.enabled = true;
    }

    public void DisableMovement()
    {
        characterMovementManager.enabled = false;
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
