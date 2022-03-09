using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterId
    {
        NONE,
        BARALD, 
        ILONA
    }

    public CharacterId characterId;
    public CharacterManager characterManager;
}
