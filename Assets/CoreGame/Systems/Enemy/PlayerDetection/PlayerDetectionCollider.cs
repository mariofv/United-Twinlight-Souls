using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionCollider : MonoBehaviour
{
    private bool isPlayerInsideCollider = false;

    public UnityEvent<Transform> onPlayerDetected;
    public UnityEvent onPlayerLost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER))
        {
            isPlayerInsideCollider = true;
            onPlayerDetected.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER))
        {
            if (!isPlayerInsideCollider)
            {
                throw new UnityException("Player left detecten but was not in collider!");
            }
            isPlayerInsideCollider = false;
            onPlayerLost.Invoke();
        }
    }
}
