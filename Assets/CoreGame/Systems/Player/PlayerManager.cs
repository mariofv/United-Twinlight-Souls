using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Character.CharacterId controlledCharacterId;

    [SerializeField] private Character barald;
    [SerializeField] private Character ilona;

    private CharacterManager controlledCharacterManager;

    public void ControlBarald()
    {
        ChangeControlledCharacter(barald);
        controlledCharacterId = Character.CharacterId.BARALD;
    }

    public void ControlIlona()
    {
        ChangeControlledCharacter(ilona);
        controlledCharacterId = Character.CharacterId.ILONA;
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
        controlledCharacterId = Character.CharacterId.NONE;
    }

    public bool HasCharacterSelected()
    {
        return controlledCharacterId != Character.CharacterId.NONE;
    }

    public CharacterManager GetControlledCharacter()
    {
        return controlledCharacterManager;
    }

    public Character.CharacterId GetControlledCharacterId()
    {
        return controlledCharacterId;
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
