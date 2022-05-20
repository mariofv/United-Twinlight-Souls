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
        ATTACK,
        SHIELD,
        INTERACT, 
        LOCK_ENEMY
    }

    private Dictionary<CharacterManager.CharacterState, List<CharacterInputAction>> characterStateAcceptedInputs;

    public Vector2 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacterStateAccepetedInputs();
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
        idleAccepetedInputs.Add(CharacterInputAction.ATTACK);
        idleAccepetedInputs.Add(CharacterInputAction.SHIELD);
        idleAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.IDLE, idleAccepetedInputs);

        List<CharacterInputAction> movingAccepetedInputs = new List<CharacterInputAction>();
        movingAccepetedInputs.Add(CharacterInputAction.INTERACT);
        movingAccepetedInputs.Add(CharacterInputAction.MOVE);
        movingAccepetedInputs.Add(CharacterInputAction.JUMP);
        movingAccepetedInputs.Add(CharacterInputAction.ATTACK);
        idleAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        movingAccepetedInputs.Add(CharacterInputAction.SHIELD);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.MOVING, movingAccepetedInputs);

        List<CharacterInputAction> jumpingAccepetedInputs = new List<CharacterInputAction>();
        jumpingAccepetedInputs.Add(CharacterInputAction.MOVE);
        jumpingAccepetedInputs.Add(CharacterInputAction.SHIELD);
        idleAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.JUMPING, jumpingAccepetedInputs);

        List<CharacterInputAction> attackngAccepetedInputs = new List<CharacterInputAction>();
        attackngAccepetedInputs.Add(CharacterInputAction.ATTACK);
        idleAccepetedInputs.Add(CharacterInputAction.LOCK_ENEMY);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.ATTACKING, attackngAccepetedInputs);

        List<CharacterInputAction> interactingAcceptedInputs = new List<CharacterInputAction>();
        interactingAcceptedInputs.Add(CharacterInputAction.INTERACT);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.INTERACTING, interactingAcceptedInputs);

        List<CharacterInputAction> deadAcceptedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.DEAD, deadAcceptedInputs);

        List<CharacterInputAction> stunedAcceptedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.STUNNED, stunedAcceptedInputs);
    }

    public bool IsInputAcceptedInCurrentState(CharacterInputAction inputAction)
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
