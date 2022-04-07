using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroplantAI : EnemyAI
{
    private enum NecroplantState
    {
        SPAWN,
        TELEPORT_OUT,
        IDLE,
        AIMING_PLAYER,
        SIMPLE_ATTACK,
        SHOTGUN_ATTACK,
        BARRAGE_ATTACK,
        DYING
    }

    private NecroplantState currentState;
    private float aiTimer = 0f;
    private bool playerInSight = false;

    [Header("Components")]
    [SerializeField] private Animator rootsAnimatorController;

    [Header("Aiming State")]
    [SerializeField] private float maxAimingTime;

    void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);
    }

    public override void Reanimate()
    {
        aiTimer = 0f;
        currentState = NecroplantState.IDLE;
    }

    protected override void UpdateAI()
    {
        switch (currentState)
        {
            case NecroplantState.SPAWN:
                break;
            case NecroplantState.TELEPORT_OUT:
                break;
            case NecroplantState.IDLE:
                break;
            case NecroplantState.AIMING_PLAYER:
                break;
            case NecroplantState.SIMPLE_ATTACK:
                break;
            case NecroplantState.SHOTGUN_ATTACK:
                break;
            case NecroplantState.BARRAGE_ATTACK:
                break;
            case NecroplantState.DYING:
                break;
        }
    }

    public override void OnSpawnStart()
    {
        currentState = NecroplantState.SPAWN;
        enemy.TriggerAnimation("spawn");
        enemy.SetInvincible(true);
    }

    public void OnSpawnEnd()
    {
        if (currentState != NecroplantState.SPAWN)
        {
            throw new UnityException("OnSpawnEnd was captured but Necroplant was in " + currentState + " state!");
        }

        enemy.SetInvincible(false);
        if (!playerInSight)
        {
            TransitionToIdleState();
        }
        else
        {
            TransitionToAimingState();
        }
    }

    private void TransitionToIdleState()
    {
        currentState = NecroplantState.IDLE;
    }

    private void TransitionToAimingState()
    {
        currentState = NecroplantState.AIMING_PLAYER;
        aiTimer = 0f;
    }

    private void OnPlayerDetected()
    {
        playerInSight = true;
        if (currentState == NecroplantState.IDLE)
        {
            TransitionToAimingState();
        }
    }

    private void OnPlayerLost()
    {
        playerInSight = false;
        if (currentState == NecroplantState.AIMING_PLAYER)
        {
            TransitionToIdleState();
        }
    }

    public override void OnDeathStart()
    {
        currentState = NecroplantState.DYING;
        DisableNavMeshAgent();
        enemy.TriggerAnimation("die");
    }

    public void OnDeathEnd()
    {
        if (currentState != NecroplantState.DYING)
        {
            throw new UnityException("OnDeathEnd called in state " + currentState);
        }

        enemy.Kill();
    }

    public override void OnHitStart()
    {

    }
}
