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
        BEING_PICKED,
        DESPAWNING
    }

    [SerializeField] protected SphereCollider pickupCollider;

    [Header("Spawning state")]
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnSpreadDistance;
    
    [Header("Pickeable state")]
    [SerializeField] protected float pickeableTime;
    [SerializeField] private float pickeableOscillationSpeed;
    [SerializeField] private float pickeableMaxOscillationHeight;

    private Vector3 originalSpeed;
    private Vector3 originalSpawnPosition;
    private Vector3 targetSpawnPosition;
    private float currentTime = 0f;

    protected PickupState currentState;


    private const float GRAVITY = 9.81f;
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

            case PickupState.BEING_PICKED:
                UpdateBeingPickedState();
                break;

            case PickupState.DESPAWNING:
                UpdateDespawningState();
                break;
        }
    }
    public void Spawn(Vector3 position)
    {
        originalSpawnPosition = position;
        targetSpawnPosition = NavMeshHelper.GetNearPosition(position, spawnSpreadDistance);
        Debug.Log("Target position is " + targetSpawnPosition);

        originalSpeed = (targetSpawnPosition - originalSpawnPosition) / spawnTime;
        originalSpeed.y = GRAVITY * 0.5f * spawnTime;

        transform.position = position;
        currentTime = 0f;
        currentState = PickupState.SPAWNING;
    }

    private void Pick()
    {
        ApplyEffect();
        TransitionToBeingPickedState();
    }
    protected abstract void ApplyEffect();

    private void UpdateSpawningState()
    {
        currentTime += Time.deltaTime;
        Vector3 nextPosition = originalSpawnPosition + originalSpeed * currentTime;
        //nextPosition.y = Mathf.Clamp(originalSpawnPosition.y + originalSpeed.y * currentTime - GRAVITY * 0.5f * currentTime * currentTime, targetSpawnPosition.y, 100f);
        nextPosition.y = originalSpawnPosition.y + originalSpeed.y * currentTime - GRAVITY * 0.5f * currentTime * currentTime;
        transform.position = nextPosition;

        if (nextPosition.y <= targetSpawnPosition.y && currentTime > spawnTime)
        {
            transform.position = targetSpawnPosition;
            TransitionToPickeableState();
        }
    }

    private void TransitionToPickeableState()
    {
        pickupCollider.enabled = true;

        currentTime = 0f;
        currentState = PickupState.PICKEABLE;
    }

    private void UpdatePickeableState()
    {
        currentTime += Time.deltaTime;
        float newHeight = (Mathf.Sin(currentTime * pickeableOscillationSpeed) + 1) * 0.5f * pickeableMaxOscillationHeight;

        Vector3 newPosition = targetSpawnPosition;
        newPosition.y = targetSpawnPosition.y + newHeight;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

        if (currentTime >= pickeableTime)
        {
            TransitionToDespawningState();
        }
    }

    private void TransitionToDespawningState()
    {
        pickupCollider.enabled = false;
        currentState = PickupState.DESPAWNING;
    }

    private void UpdateDespawningState()
    {
        TransitionToDisabledState();
    }

    private void TransitionToBeingPickedState()
    {
        pickupCollider.enabled = false;
        currentState = PickupState.BEING_PICKED;
    }

    private void UpdateBeingPickedState()
    {
        TransitionToDisabledState();
    }

    private void TransitionToDisabledState()
    {
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
            //Pick();
        }
    }
}
