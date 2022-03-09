using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushdoomAI : EnemyAI
{
    private enum MushdoomState
    {
        IDLE,
        CHASING_PLAYER,
        SPIN_ATTACK,
        SPORE_ATTACK
    }

    private MushdoomState currentState;

    void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MushdoomState.IDLE:
                UpdateIdleState();
                break;

            case MushdoomState.CHASING_PLAYER:
                UpdateChasingPlayerState();
                break;

            case MushdoomState.SPIN_ATTACK:
                UpdateAttackingState();
                break;

            case MushdoomState.SPORE_ATTACK:
                break;
        }
    }

    private void UpdateIdleState()
    {
    }

    private void UpdateChasingPlayerState()
    {
        enemyNavMeshAgent.SetDestination(GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition());
    }

    private void UpdateAttackingState()
    {

    }

    private void OnPlayerDetected()
    {
        if (currentState == MushdoomState.IDLE)
        {
            currentState = MushdoomState.CHASING_PLAYER;
        }
    }

    private void OnPlayerLost()
    {
        if (currentState == MushdoomState.CHASING_PLAYER)
        {
            currentState = MushdoomState.IDLE;
        }
    }
}
