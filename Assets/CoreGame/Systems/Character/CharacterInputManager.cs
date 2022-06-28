using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputManager : CharacterSubManager
{
    public enum CharacterInputAction
    {
        MOVE,
        JUMP,
        LIGHT_ATTACK,
        SPECIAL_ATTACK,
        SHIELD,
        INTERACT, 
        LOCK_ENEMY,
        DASH
    }

    private Dictionary<CharacterManager.CharacterState, List<CharacterInputAction>> characterStateAcceptedInputs;
    private Dictionary<CharacterInputAction, ProgressionManager.Progression> progressionAcceptedInputs;

    public Vector2 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacterStateAccepetedInputs();
        InitializeProgressionAccepetedInputs();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputAcceptedInCurrentState(CharacterInputAction.MOVE))
        {
            characterManager.Move(movementVector);
        }
    }

    private void InitializeCharacterStateAccepetedInputs()
    {
        characterStateAcceptedInputs = new Dictionary<CharacterManager.CharacterState, List<CharacterInputAction>>();

        List<CharacterInputAction> idleAccepetedInputs = new List<CharacterInputAction>();
        idleAccepetedInputs.Add(CharacterInputAction.INTERACT);
        idleAccepetedInputs.Add(CharacterInputAction.MOVE);
        idleAccepetedInputs.Add(CharacterInputAction.JUMP);
        idleAccepetedInputs.Add(CharacterInputAction.LIGHT_ATTACK);
        idleAccepetedInputs.Add(CharacterInputAction.DASH);
        idleAccepetedInputs.Add(CharacterInputAction.SHIELD);
        idleAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.IDLE, idleAccepetedInputs);

        List<CharacterInputAction> movingAccepetedInputs = new List<CharacterInputAction>();
        movingAccepetedInputs.Add(CharacterInputAction.INTERACT);
        movingAccepetedInputs.Add(CharacterInputAction.MOVE);
        movingAccepetedInputs.Add(CharacterInputAction.JUMP);
        movingAccepetedInputs.Add(CharacterInputAction.LIGHT_ATTACK);
        movingAccepetedInputs.Add(CharacterInputAction.DASH);
        movingAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        movingAccepetedInputs.Add(CharacterInputAction.SHIELD);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.MOVING, movingAccepetedInputs);

        List<CharacterInputAction> jumpingAccepetedInputs = new List<CharacterInputAction>();
        jumpingAccepetedInputs.Add(CharacterInputAction.MOVE);
        jumpingAccepetedInputs.Add(CharacterInputAction.SHIELD);
        jumpingAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        jumpingAccepetedInputs.Add(CharacterInputAction.DASH);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.JUMPING, jumpingAccepetedInputs);

        List<CharacterInputAction> lightAttackingAccepetedInputs = new List<CharacterInputAction>();
        lightAttackingAccepetedInputs.Add(CharacterInputAction.LIGHT_ATTACK);
        lightAttackingAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.LIGHT_ATTACKING, lightAttackingAccepetedInputs);

        List<CharacterInputAction> specialAttackingAccepetedInputs = new List<CharacterInputAction>();
        specialAttackingAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.SPECIAL_ATTACKING, specialAttackingAccepetedInputs);

        List<CharacterInputAction> dashingAccepetedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.DASHING, dashingAccepetedInputs);

        List<CharacterInputAction> interactingAcceptedInputs = new List<CharacterInputAction>();
        interactingAcceptedInputs.Add(CharacterInputAction.INTERACT);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.INTERACTING, interactingAcceptedInputs);

        List<CharacterInputAction> deadAcceptedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.DEAD, deadAcceptedInputs);

        List<CharacterInputAction> stunedAcceptedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.STUNNED, stunedAcceptedInputs);
    }

    private void InitializeProgressionAccepetedInputs()
    {
        progressionAcceptedInputs = new Dictionary<CharacterInputAction, ProgressionManager.Progression>();

        progressionAcceptedInputs.Add(CharacterInputAction.LIGHT_ATTACK, ProgressionManager.Progression.LIGHT_ATTACK_UNLOCKED);
        progressionAcceptedInputs.Add(CharacterInputAction.SPECIAL_ATTACK, ProgressionManager.Progression.SPECIAL_ATTACK_UNLOCKED);
        progressionAcceptedInputs.Add(CharacterInputAction.DASH, ProgressionManager.Progression.DASH_UNLOCKED);
        progressionAcceptedInputs.Add(CharacterInputAction.LOCK_ENEMY, ProgressionManager.Progression.LOCK_ON_UNLOCKED);
        progressionAcceptedInputs.Add(CharacterInputAction.SHIELD, ProgressionManager.Progression.SHIELD_UNLOCKED);
    }

    public bool IsInputAccepted(CharacterInputAction inputAction)
    {
        return IsInputAcceptedInCurrentProgression(inputAction) && IsInputAcceptedInCurrentState(inputAction);
    }

    private bool IsInputAcceptedInCurrentProgression(CharacterInputAction inputAction)
    {
        if (!progressionAcceptedInputs.ContainsKey(inputAction))
        {
            return true;
        }

        return GameManager.instance.progressionManager.CheckProgression(progressionAcceptedInputs[inputAction]);
    }

    private bool IsInputAcceptedInCurrentState(CharacterInputAction inputAction)
    {
        CharacterManager.CharacterState currentCharacterState = characterManager.GetCharacterState();

        foreach (CharacterInputAction acceptedAction in characterStateAcceptedInputs[currentCharacterState])
        {
            if (acceptedAction == inputAction)
            {
                return true;
            }
        }

        return false;
    }
}
