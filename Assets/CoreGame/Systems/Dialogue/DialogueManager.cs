using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueAsset currentDialogue;
    private int currentDialogueMessage;

    public void StartDialogue(DialogueAsset dialogue)
    {
        GameManager.instance.EnterGameState(GameManager.GameState.DIALOGUE);

        currentDialogue = dialogue;
        currentDialogueMessage = -1;
        NextDialogueMessage();
    }

    public void NextDialogueMessage()
    {
        ++currentDialogueMessage;
        GameManager.instance.uiManager.gameUIManager.dialogueUI.DisplayDialogueText(currentDialogue.dialogueMessages[currentDialogueMessage]);
    }

    public void EndCurrentDialogue()
    {
        currentDialogue = null;
        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
    }

    public bool CanAdvanceInDialogue()
    {
        return currentDialogueMessage < currentDialogue.dialogueMessages.Count - 1;
    }

    public bool IsPlayerInDialogue()
    {
        return currentDialogue != null;
    }
}
