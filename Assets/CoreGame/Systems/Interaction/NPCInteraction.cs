using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NPCInteraction : MonoBehaviour
{
    private enum InteractionState
    {
        HIDDEN,
        SHOWING,
        AVAILABLE,
        INTERACTING,
        HIDING
    }

    [Header("NPC")]
    [SerializeField] private NPC npc;
    [SerializeField] private SkinnedMeshRenderer npcRenderer;
    [SerializeField] private GameObject npcOrb;


    [Header("Interaction")]
    [SerializeField] private DialogueAsset interactionDialogue;
    [SerializeField] private CinemachineVirtualCamera interactionCamera;
    [SerializeField] private float transitionTime;
    [SerializeField] private Tutorial interactionTutorial;

    private InteractionState currentState;
    private float currentTime = 0f;

    private bool hasPlayerInteracted = false;

    private void Awake()
    {
        npcRenderer.material.SetFloat("_FadeProgress", 1f);
    }

    private void Update()
    {
        switch (currentState)
        {
            case InteractionState.HIDDEN:
                break;
            case InteractionState.SHOWING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(1f, currentTime / transitionTime);

                    npcRenderer.material.SetFloat("_FadeProgress", 1f - progress);

                    if (progress == 1f)
                    {
                        currentState = InteractionState.AVAILABLE;
                        npcOrb.SetActive(true);
                    }
                }
                break;
            case InteractionState.AVAILABLE:
                break;
            case InteractionState.INTERACTING:
                break;
            case InteractionState.HIDING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(1f, currentTime / transitionTime);

                    npcRenderer.material.SetFloat("_FadeProgress", progress);

                    if (progress == 1f)
                    {
                        currentState = InteractionState.HIDDEN;
                        npcOrb.SetActive(false);
                    }
                }
                break;
        }
    }

    public void StartInteraction()
    {
        GameManager.instance.EnterGameState(GameManager.GameState.DIALOGUE);
        GameManager.instance.dialogueManager.StartDialogue(interactionDialogue);
        GameManager.instance.dialogueManager.onDialogueEnd.AddListener(EndInteraction);

        GameManager.instance.player.GetControlledCharacter().characterVisualsManager.HideMesh();
        GameManager.instance.player.GetControlledCharacter().characterMovementManager.Orientate(transform);
        GameManager.instance.cameraManager.LoadCamera(interactionCamera);
        npc.LookAtTransform(interactionCamera.transform);

        currentState = InteractionState.INTERACTING;
    }

    private void EndInteraction()
    {

        GameManager.instance.player.GetControlledCharacter().characterVisualsManager.ShowMesh();
        GameManager.instance.cameraManager.LoadCamera(GameManager.instance.levelManager.GetCurrentLevel().GetCurrentCamera());
        npc.LookAtPlayer();

        GameManager.instance.dialogueManager.onDialogueEnd.RemoveListener(EndInteraction);
        GameManager.instance.tutorialManager.StartTutorial(interactionTutorial);
        interactionTutorial.onTutorialEnd.AddListener(Hide);
        
        hasPlayerInteracted = true;
        currentState = InteractionState.AVAILABLE;
        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT);
    }

    public bool IsAvailable()
    {
        return (currentState == InteractionState.AVAILABLE || currentState == InteractionState.INTERACTING) && !hasPlayerInteracted;
    }

    public void Show()
    {
        if (hasPlayerInteracted)
        {
            return;
        }

        currentState = InteractionState.SHOWING;
        currentTime = 0f;
    }

    public void Hide()
    {
        interactionTutorial.onTutorialEnd.RemoveListener(Hide);

        currentState = InteractionState.HIDING;
        currentTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            Show();
        }
    }
}
