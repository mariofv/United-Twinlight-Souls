using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Character barald;
    [SerializeField] private Character furra;

    private CharacterManager controlledCharacterManager;

    public void Awake()
    {
        ControllBarald();
    }

    public void ControllBarald()
    {
        ChangeControlledCharacter(barald);
    }

    public void ChangeControlledCharacter(Character character)
    {
        controlledCharacterManager = character.characterManager;
    }

    public CharacterManager Character()
    {
        return controlledCharacterManager;
    }
}
