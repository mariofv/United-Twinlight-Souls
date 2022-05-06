using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private DialogueAsset interactionDialogue;

    public void Interact()
    {
        GameManager.instance.dialogueManager.StartDialogue(interactionDialogue);
    }
}
