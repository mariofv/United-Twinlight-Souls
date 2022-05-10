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
        INTERACT
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
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.IDLE, idleAccepetedInputs);

        List<CharacterInputAction> movingAccepetedInputs = new List<CharacterInputAction>();
        movingAccepetedInputs.Add(CharacterInputAction.INTERACT);
        movingAccepetedInputs.Add(CharacterInputAction.MOVE);
        movingAccepetedInputs.Add(CharacterInputAction.JUMP);
        movingAccepetedInputs.Add(CharacterInputAction.ATTACK);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.MOVING, movingAccepetedInputs);

        List<CharacterInputAction> jumpingAccepetedInputs = new List<CharacterInputAction>();
        jumpingAccepetedInputs.Add(CharacterInputAction.MOVE);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.JUMPING, jumpingAccepetedInputs);

        List<CharacterInputAction> attackngAccepetedInputs = new List<CharacterInputAction>();
        attackngAccepetedInputs.Add(CharacterInputAction.ATTACK);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.ATTACKING, attackngAccepetedInputs);

        List<CharacterInputAction> interactingAcceptedInputs = new List<CharacterInputAction>();
        interactingAcceptedInputs.Add(CharacterInputAction.INTERACT);
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.INTERACTING, interactingAcceptedInputs);

        List<CharacterInputAction> dyingAcceptedInputs = new List<CharacterInputAction>();
        characterStateAcceptedInputs.Add(CharacterManager.CharacterState.DYING, dyingAcceptedInputs);
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
