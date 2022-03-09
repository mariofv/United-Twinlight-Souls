using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MushdoomAI : EnemyAI
{
    private enum MushdoomState
    {
        IDLE,
        WANDERING,
        CHASING_PLAYER,
        SPIN_ATTACK,
        SPORE_ATTACK
    }

    [Header("Idle State")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;

    [Header("Wandering State")]
    [SerializeField] private float maxWanderingTime;
    [SerializeField] private float nextWanderingPositionDistance;

    private float chasingPlayerStopDistance;

    private MushdoomState currentState;

    private Vector3 nextWanderingDestination;
    private float aiTimer = 0f;
    private float idleTime;

    void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);

        chasingPlayerStopDistance = enemyNavMeshAgent.stoppingDistance;
    }

    protected override void UpdateAI()
    {
        switch (currentState)
        {
            case MushdoomState.IDLE:
                UpdateIdleState();
                break;

            case MushdoomState.WANDERING:
                UpdateWanderingState();
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

    private void TransitionToIdleState()
    {
        enemyNavMeshAgent.ResetPath();
        idleTime = Random.Range(minIdleTime, maxIdleTime);

        aiTimer = 0f;
        currentState = MushdoomState.IDLE;
    }

    private void UpdateIdleState()
    {
        aiTimer += deltaTimeAI;
        if (aiTimer >= idleTime)
        {
            TransitionToWanderingState();
        }
    }

    private void TransitionToWanderingState()
    {
        nextWanderingDestination = NavMeshHelper.GetNearPosition(transform.position, nextWanderingPositionDistance);
        enemyNavMeshAgent.stoppingDistance = 0f;
        enemyNavMeshAgent.SetDestination(nextWanderingDestination);

        aiTimer = 0f;
        currentState = MushdoomState.WANDERING;
    }

    private void UpdateWanderingState()
    {
        aiTimer += deltaTimeAI;
        if (Vector3.SqrMagnitude(transform.position - nextWanderingDestination) <= 0.5f || aiTimer >= maxWanderingTime)
        {
            TransitionToIdleState();
        }
    }

    private void TransitionToChasingState()
    {
        currentState = MushdoomState.CHASING_PLAYER;
        enemyNavMeshAgent.stoppingDistance = chasingPlayerStopDistance;
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
        if (IsInPassiveState())
        {
            TransitionToChasingState();
        }
    }

    private void OnPlayerLost()
    {
        if (IsInCombatState())
        {
            TransitionToIdleState();
        }
    }

    private bool IsInPassiveState()
    {
        return
            currentState == MushdoomState.IDLE
            || currentState == MushdoomState.WANDERING;
    }

    private bool IsInCombatState()
    {
        return
            currentState == MushdoomState.CHASING_PLAYER
            || currentState == MushdoomState.SPIN_ATTACK
            || currentState == MushdoomState.SPORE_ATTACK;
    }

    private void OnDrawGizmosSelected()
    {
        if (!GameManager.instance.debugManager.showEnemyDebugInfo)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(nextWanderingDestination, 1f);
        Gizmos.color = Color.white;
    }
}
