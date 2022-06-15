using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSubManager : MonoBehaviour
{
    protected CharacterManager characterManager;

    public void Link(CharacterManager characterManager)
    {
        this.characterManager = characterManager;
    }
}
