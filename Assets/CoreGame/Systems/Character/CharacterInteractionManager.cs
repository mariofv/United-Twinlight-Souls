using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionManager : CharacterSubManager
{
    private NPCInteraction npcInteraction;

    public void Interact()
    {
        if (GameManager.instance.dialogueManager.IsPlayerInDialogue())
        {
            if (GameManager.instance.dialogueManager.CanAdvanceInDialogue())
            {
                GameManager.instance.dialogueManager.NextDialogueMessage();
            }
            else
            {
                GameManager.instance.dialogueManager.EndCurrentDialogue();
                characterManager.SetCharacterState(CharacterManager.CharacterState.IDLE);
            }
        }
        else
        {
            characterManager.Move(Vector2.zero);
            characterManager.SetCharacterState(CharacterManager.CharacterState.INTERACTING);
            npcInteraction.StartInteraction();
        }
    }

    public bool CanInteract()
    {
        return npcInteraction != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.NPC))
        {
            npcInteraction = other.GetComponent<NPC>().GetInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.NPC))
        {
            npcInteraction = null;
        }
    }
}
