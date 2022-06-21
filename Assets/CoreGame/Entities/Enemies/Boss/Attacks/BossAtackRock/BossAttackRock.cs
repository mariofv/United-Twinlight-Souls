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

                        newHeight = Mathf.Lerp(spawnFromGroundHeight, 1.5f, progress);
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
                    }

                }
                break;

            case AttackRockState.EXPLODING:
                currentState = AttackRockState.INACTIVE;
                break;
        }
    }

    public void Spawn(Vector3 position, bool isSpawningFromGround)
    {
        currentState = AttackRockState.SPAWNING;
        currentTime = 0f;

        transform.position = position;

        this.isSpawningFromGround = isSpawningFromGround;
        if (isSpawningFromGround)
        {
            rockTransform.position = rockTransform.position - Vector3.up * spawnFromGroundHeight;
            rockTransform.rotation = Quaternion.identity;
        }
        else
        {
            rockTransform.position = rockTransform.position + Vector3.up * spawnFromCellingHeight;
            rockTransform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }

    }

    public bool IsActive()
    {
        return currentState != AttackRockState.INACTIVE;
    }
}
