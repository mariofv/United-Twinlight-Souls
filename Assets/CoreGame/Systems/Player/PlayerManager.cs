using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Character barald;
    [SerializeField] private Character ilona;

    private CharacterManager controlledCharacterManager;

    public void ControlBarald()
    {
        ChangeControlledCharacter(barald);
    }

    public void ControlIlona()
    {
        ChangeControlledCharacter(ilona);
    }

    public void ChangeControlledCharacter(Character character)
    {
        controlledCharacterManager = character.characterManager;
    }

    public void DeselectCurrentCharacter()
    {
        if (controlledCharacterManager == null)
        {
            return;
        }

        controlledCharacterManager.DisableMovement();
        controlledCharacterManager = null;
    }

    public bool HasCharacterSelected()
    {
        return controlledCharacterManager != null;
    }

    public CharacterManager Character()
    {
        return controlledCharacterManager;
    }

    public CharacterManager GetBarald()
    {
        return barald.characterManager;
    }

    public CharacterManager GetIlona()
    {
        return ilona.characterManager;
    }
}
