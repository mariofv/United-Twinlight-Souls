using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private NPC npc;
    [SerializeField] private DialogueAsset interactionDialogue;
    [SerializeField] private CinemachineVirtualCamera interactionCamera;

    public void StartInteraction()
    {
        GameManager.instance.EnterGameState(GameManager.GameState.DIALOGUE);
        GameManager.instance.dialogueManager.StartDialogue(interactionDialogue);
        GameManager.instance.dialogueManager.onDialogueEnd.AddListener(EndInteraction);

        GameManager.instance.player.GetControlledCharacter().characterVisualsManager.HideMesh();
        GameManager.instance.cameraManager.LoadCamera(interactionCamera);
        npc.LookAtTransform(interactionCamera.transform);
    }

    private void EndInteraction()
    {
        GameManager.instance.player.GetControlledCharacter().characterVisualsManager.ShowMesh();
        GameManager.instance.cameraManager.LoadCamera(GameManager.instance.levelManager.GetCurrentLevel().GetCurrentCamera());
        npc.LookAtPlayer();

        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
    }
}
