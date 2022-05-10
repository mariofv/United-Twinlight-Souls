using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool alreadyReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyReached && other.CompareTag(TagManager.PLAYER))
        {
            alreadyReached = true;
            GameManager.instance.levelManager.SetLastCheckpoint(this);
        }
    }
}
