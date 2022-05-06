using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionManager : CharacterSubManager
{
    private NPCInteraction npcInteraction;

    public void Interact()
    {
        if (npcInteraction != null)
        {
            npcInteraction.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.NPC))
        {
            npcInteraction = other.GetComponent<NPCInteraction>();
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
