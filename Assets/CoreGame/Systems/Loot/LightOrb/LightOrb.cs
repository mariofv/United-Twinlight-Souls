using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : Pickup
{
    [SerializeField] private int healedAmount;

    [Header("Spawn parameters")]
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnSpreadDistance;
    private Vector3 originalSpeed;
    private Vector3 originalSpawnPosition;
    private Vector3 targetSpawnPosition;
    private float currentTime = 0f;


    private const float GRAVITY = 9.81f;

    protected override void ApplyEffect()
    {
        GameManager.instance.player.GetControlledCharacter().characterStatsManager.Heal(healedAmount);
    }

    public override void Spawn(Vector3 position)
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

    protected override void UpdateSpawningState()
    {
        currentTime += Time.deltaTime;
        Vector3 nextPosition = originalSpawnPosition + originalSpeed * currentTime;
        //nextPosition.y = Mathf.Clamp(originalSpawnPosition.y + originalSpeed.y * currentTime - GRAVITY * 0.5f * currentTime * currentTime, targetSpawnPosition.y, 100f);
        nextPosition.y = originalSpawnPosition.y + originalSpeed.y * currentTime - GRAVITY * 0.5f * currentTime * currentTime;
        transform.position = nextPosition;

        if (nextPosition.y <= targetSpawnPosition.y && currentTime > spawnTime)
        {
            TransitionToPickeableState();
        }
    }

    protected void TransitionToPickeableState()
    {
        pickupCollider.enabled = true;
        currentState = PickupState.PICKEABLE;
    }

    protected override void UpdatePickeableState()
    {

    }

    protected override void UpdateDespawningState()
    {
        TransitionToDisabledState();
    }

    protected override void TransitionToDespawningState()
    {
        currentState = PickupState.DESPAWNING;
    }
}
