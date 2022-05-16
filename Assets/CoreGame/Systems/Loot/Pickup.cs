using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected enum PickupState
    {
        DISABLED,
        SPAWNING,
        PICKEABLE,
        DESPAWNING
    }

    [SerializeField] protected SphereCollider pickupCollider;
    protected PickupState currentState;

    private void Update()
    {
        switch (currentState)
        {
            case PickupState.DISABLED:
                break;
            case PickupState.SPAWNING:
                UpdateSpawningState();
                break;
            case PickupState.PICKEABLE:
                UpdatePickeableState();
                break;
            case PickupState.DESPAWNING:
                UpdateDespawningState();
                break;
        }
    }


    private void Pick()
    {
        ApplyEffect();
        TransitionToDespawningState();
    }

    public abstract void Spawn(Vector3 position);
    protected abstract void ApplyEffect();

    protected abstract void UpdateSpawningState();
    protected abstract void UpdatePickeableState();
    protected abstract void UpdateDespawningState();

    protected abstract void TransitionToDespawningState();
    protected void TransitionToDisabledState()
    {
        pickupCollider.enabled = false;
        currentState = PickupState.DISABLED;
    }

    public bool IsActive()
    {
        return currentState != PickupState.DISABLED;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            Pick();
        }
    }
}
