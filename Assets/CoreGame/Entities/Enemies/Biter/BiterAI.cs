using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiterAI : EnemyAI
{
    private enum BiterState
    {
        SPAWN,
        IDLE,
        CHASING_PLAYER,
        BITING,
        HIT,
        DYING
    }

    private BiterState currentState;
    private bool playerInSight = false;

    [Header("Floating")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float floatingAmplitude;

    [Header("Chasing State")]
    private float originalHeight;
    private float chasingPlayerStopDistance;

    [Header("Biting State")]
    [SerializeField] private int biteAttackDamage;
    [SerializeField] private BiteAttackCollider biteAttackCollider;

    private void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);

        biteAttackCollider.SetDamage(biteAttackDamage);

        chasingPlayerStopDistance = enemyNavMeshAgent.stoppingDistance;
    }

    public override void Reanimate()
    {
        playerInSight = false;
        currentState = BiterState.IDLE;
        originalHeight = bodyTransform.position.y;
    }

    protected override void UpdateSpecific()
    {
        switch (currentState)
        {
            case BiterState.SPAWN:
                break;
            case BiterState.IDLE:
                UpdateFloatingPosition();
                break;
            case BiterState.CHASING_PLAYER:
                UpdateFloatingPosition();
                break;
            case BiterState.BITING:
                UpdateFloatingPosition();
                break;
            case BiterState.HIT:
                UpdateFloatingPosition();
                break;
            case BiterState.DYING:
                break;
        }
    }

    protected override void UpdateAI()
    {
        switch (currentState)
        {
            case BiterState.SPAWN:
                break;
            case BiterState.IDLE:
                break;
            case BiterState.CHASING_PLAYER:
                UpdateChasingPlayerAIState();
                break;
            case BiterState.HIT:
                break;
            case BiterState.DYING:
                break;
        }
    }

    public override void OnSpawnStart()
    {
        currentState = BiterState.SPAWN;
        enemy.SetInvincible(true);
        OnSpawnEnd();
    }

    public void OnSpawnEnd()
    {
        if (currentState != BiterState.SPAWN)
        {
            throw new UnityException("OnSpawnEnd was captured but Biter was in " + currentState + " state!");
        }

        enemy.SetInvincible(false);
        if (!playerInSight)
        {
            TransitionToIdleState();
        }
        else
        {
            TransitionToChasingState();
        }
    }

    private void TransitionToIdleState()
    {
        currentState = BiterState.IDLE;
    }

    private void TransitionToChasingState()
    {
        currentState = BiterState.CHASING_PLAYER;
        enemyNavMeshAgent.stoppingDistance = chasingPlayerStopDistance;
    }

    private void UpdateFloatingPosition()
    {
        Vector3 newPosition = bodyTransform.position;
        newPosition.y = originalHeight + Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude;

        bodyTransform.position = newPosition;
    }

    private void UpdateChasingPlayerAIState()
    {
        Vector3 playerPosition = GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition();
        enemyNavMeshAgent.SetDestination(playerPosition);
        if ((transform.position - playerPosition).sqrMagnitude <= (enemyNavMeshAgent.stoppingDistance * enemyNavMeshAgent.stoppingDistance))
        {
            TransitionToBiteState();
        }
    }

    private void TransitionToBiteState()
    {
        enemy.TriggerAnimation("bite");
        currentState = BiterState.BITING;
        biteAttackCollider.SetColliderActive(true);
    }

    public void OnBiteAttackEnd()
    {
        biteAttackCollider.SetColliderActive(false);

        if (currentState != BiterState.BITING)
        {
            return;
        }

        if (!playerInSight)
        {
            TransitionToIdleState();
        }
        else
        {
            TransitionToChasingState();
        }
    }

    public override void OnHitStart()
    {
        currentState = BiterState.HIT;
        enemy.TriggerAnimation("hitReaction");
    }

    public void OnHitEnd()
    {
        if (currentState != BiterState.HIT)
        {
            return;
        }

        if (!playerInSight)
        {
            TransitionToIdleState();
        }
        else
        {
            TransitionToChasingState();
        }
    }

    public override void OnDeathStart()
    {
        currentState = BiterState.DYING;
        DisableNavMeshAgent();
        enemy.TriggerAnimation("die");
    }

    public void OnDeathEnd()
    {
        if (currentState != BiterState.DYING)
        {
            throw new UnityException("OnDeathEnd called in state " + currentState);
        }

        enemy.Kill();
    }

    private void OnPlayerDetected()
    {
        playerInSight = true;
        if (IsInPassiveState())
        {
            TransitionToChasingState();
        }
    }

    private void OnPlayerLost()
    {
        playerInSight = false;
        if (IsInChasingState())
        {
            TransitionToIdleState();
        }
    }

    private bool IsInPassiveState()
    {
        return
            currentState == BiterState.IDLE;
        //|| currentState == BiterState.WANDERING;
    }

    private bool IsInChasingState()
    {
        return currentState == BiterState.CHASING_PLAYER;
    }

}
