using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiterAI : EnemyAI
{
    private enum BiterState
    {
        SPAWN,
        IDLE,
        HIT,
        DYING
    }

    private BiterState currentState;
    private bool playerInSight = false;

    private void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);
    }

    public override void Reanimate()
    {
        playerInSight = false;
        currentState = BiterState.IDLE;
    }

    protected override void UpdateAI()
    {
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
        /*
        currentState = MushdoomState.CHASING_PLAYER;
        enemyNavMeshAgent.stoppingDistance = chasingPlayerStopDistance;
        */
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
        //playerInSight = true;
        //if (currentState == NecroplantState.IDLE)
        //{
        //    TransitionToAimingState();
        //}
    }

    private void OnPlayerLost()
    {
        //playerInSight = false;
        //if (currentState == NecroplantState.AIMING_PLAYER)
        //{
        //    TransitionToIdleState();
        //}
    }

}
