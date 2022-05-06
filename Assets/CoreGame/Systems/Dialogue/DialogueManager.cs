using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueAsset currentDialogue;
    private int currentDialogueMessage;

    public void StartDialogue(DialogueAsset dialogue)
    {
        currentDialogue = dialogue;
        currentDialogueMessage = -1;

        GameManager.instance.uiManager.gameUIManager.dialogueUI.Show();
        NextDialogueMessage();
    }

    public void NextDialogueMessage()
    {
        ++currentDialogueMessage;
        if (currentDialogueMessage < currentDialogue.dialogueMessages.Count)
        {
            GameManager.instance.uiManager.gameUIManager.dialogueUI.DisplayDialogueText(currentDialogue.dialogueMessages[currentDialogueMessage]);
        }
        else
        {
            EndCurrentDialogue();
        }
    }

    private void EndCurrentDialogue()
    {
        GameManager.instance.uiManager.gameUIManager.dialogueUI.Hide();
    }
}
