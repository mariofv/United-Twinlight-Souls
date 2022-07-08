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
            if (GameManager.instance.uiManager.gameUIManager.dialogueUI.IsTyping())
            {
                GameManager.instance.uiManager.gameUIManager.dialogueUI.SkipDialogueTyping();
            }
            else
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
        }
        else
        {
            characterManager.Move(Vector2.zero);
            characterManager.SetCharacterState(CharacterManager.CharacterState.INTERACTING);
            npcInteraction.StartInteraction();
            GameManager.instance.uiManager.gameUIManager.interactionUI.Hide();
        }
    }

    public bool CanInteract()
    {
        if (npcInteraction == null)
        {
            return false;
        }

        return npcInteraction.IsAvailable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.NPC))
        {
            npcInteraction = other.GetComponent<NPC>().GetInteraction();

            if (npcInteraction.IsAvailable())
            {
                GameManager.instance.uiManager.gameUIManager.interactionUI.SetTargetTransform(other.transform);
                GameManager.instance.uiManager.gameUIManager.interactionUI.Show();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.NPC))
        {
            npcInteraction = null;
            GameManager.instance.uiManager.gameUIManager.interactionUI.SetTargetTransform(null);
            GameManager.instance.uiManager.gameUIManager.interactionUI.Hide();
        }
    }
}
