using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSubManager : MonoBehaviour
{
    protected CharacterManager characterManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Link(CharacterManager characterManager)
    {
        this.characterManager = characterManager;
    }
}
