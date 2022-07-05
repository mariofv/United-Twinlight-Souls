using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRock : MonoBehaviour
{
    private enum AttackRockState
    {
        INACTIVE,
        FORESHADOWING,
        SPAWNING,
        EXPLODING
    }

    [SerializeField] private Transform rockTransform;

    [Header("Foreshadowing")]
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private float foreshadowTime;

    [Header("Ground Spawning")]
    [SerializeField] private float spawnFromGroundTime;
    [SerializeField] private float spawnFromGroundHeight;
    
    [Header("Ceilling Spawning")]
    [SerializeField] private float spawnFromCellingTime;
    [SerializeField] private float spawnFromCellingHeight;
    
    [Header("Exploding")]
    [SerializeField] private float explodingTime;

    [Header("Hitbox")]
    [SerializeField] private BoxCollider hitbox;
    [SerializeField] private int rockDamage;
    private bool hasHitPlayer = false;

    private bool isSpawningFromGround;

    private AttackRockState currentState;
    private float currentTime;

    private void Update()
    {
        switch (currentState)
        {
            case AttackRockState.INACTIVE:
                break;
            case AttackRockState.FORESHADOWING:
                {
                    currentTime += Time.deltaTime;
                    float progress = Mathf.Min(1f, currentTime / foreshadowTime);

                    shadowTransform.localScale = Vector3.one * progress;

                    if (progress == 1f)
                    {
                        currentState = AttackRockState.SPAWNING;
                        currentTime = 0f;
                    }
                }
                break;

            case AttackRockState.SPAWNING:
                {
                    currentTime += Time.deltaTime;
                    float progress;
                    float newHeight;

                    if (isSpawningFromGround)
                    {
                        progress = Mathf.Min(1f, currentTime / spawnFromGroundTime);

                        newHeight = Mathf.Lerp(-spawnFromGroundHeight, 1.5f, progress);
                    }
                    else
                    {
                        progress = Mathf.Min(1f, currentTime / spawnFromCellingTime);

                        newHeight = Mathf.Lerp(spawnFromCellingHeight, 2.5f, progress);
                    }

                    Vector3 newPosition = rockTransform.position;
                    newPosition.y = newHeight;
                    rockTransform.position = newPosition;
                    
                    if (progress == 1f)
                    {
                        currentState = AttackRockState.EXPLODING;
                        currentTime = 0f;
                        hitbox.enabled = true;
                    }

                }
                break;

            case AttackRockState.EXPLODING:
                {
                    currentTime += Time.deltaTime;

                    if (currentTime >= explodingTime)
                    {
                        currentState = AttackRockState.INACTIVE;
                        hitbox.enabled = false;
                        transform.position = GameManager.instance.levelManager.GetCurrentLevel().voidPosition.position;
                    }
                }
                break;
        }
    }

    public void Spawn(Vector3 position, bool isSpawningFromGround)
    {
        currentState = AttackRockState.FORESHADOWING;
        currentTime = 0f;
        hasHitPlayer = false;

        transform.position = position;

        this.isSpawningFromGround = isSpawningFromGround;
        Vector3 spawnPosition = rockTransform.position; 
        if (isSpawningFromGround)
        {
            spawnPosition.y = -spawnFromGroundHeight;
            rockTransform.rotation = Quaternion.identity;
        }
        else
        {
            spawnPosition.y = spawnFromCellingHeight;
            rockTransform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        rockTransform.position = spawnPosition;
    }

    public bool IsActive()
    {
        return currentState != AttackRockState.INACTIVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHitPlayer && other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(rockDamage, transform.position, attackCausesStun: false);
            hasHitPlayer = true;
        }
    }
}
