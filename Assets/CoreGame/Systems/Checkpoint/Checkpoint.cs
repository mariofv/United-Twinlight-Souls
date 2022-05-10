using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool alreadyActivated = false;

    public void ActivateCheckpoint()
    {
        alreadyActivated = true;
        GameManager.instance.levelManager.SetLastCheckpoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyActivated && other.CompareTag(TagManager.PLAYER))
        {
            ActivateCheckpoint();
        }
    }
}
