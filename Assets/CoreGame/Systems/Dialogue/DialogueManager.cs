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
        NextDialogueMessage();
    }

    public void NextDialogueMessage()
    {
        ++currentDialogueMessage;
        if (currentDialogueMessage < currentDialogue.dialogueMessages.Count)
        {

        }
        else
        {
            EndCurrentDialogue();
        }
    }

    private void EndCurrentDialogue()
    {

    }
}
