using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public UnityEvent onDialogueEnd;
    private DialogueAsset currentDialogue;
    private int currentDialogueMessage;

    public void StartDialogue(DialogueAsset dialogueAsset)
    {
        currentDialogue = dialogueAsset;
        currentDialogueMessage = -1;
        NextDialogueMessage();
    }

    public void NextDialogueMessage()
    {
        ++currentDialogueMessage;
        GameManager.instance.audioManager.PlayNPCSound(AudioManager.NPCSound.TALK);
        GameManager.instance.uiManager.gameUIManager.dialogueUI.DisplayDialogueText(currentDialogue.dialogueMessages[currentDialogueMessage]);
    }

    public void EndCurrentDialogue()
    {
        currentDialogue = null;
        onDialogueEnd.Invoke();
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
