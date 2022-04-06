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
        BATTLE_CRY,
        CHASING_PLAYER,
        SPIN_ATTACK,
        SPORE_ATTACK,
        DYING
    }

    [Header("Idle State")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleTime;

    [Header("Wandering State")]
    [SerializeField] private float maxWanderingTime;
    [SerializeField] private float nextWanderingPositionDistance;
    private Vector3 nextWanderingDestination;

    [Header("BattleCry State")]
    private bool hasBattleCried = false;

    [Header("Chasing State")]
    private float chasingPlayerStopDistance;

    [Header("Spin Attack State")]
    [SerializeField] private List<MushdoomSpinAttackCollider> spinAttackColliders;
    [SerializeField] private float spinAttackProbability;
    [SerializeField] private int spinAttackDamage;
    
    [Header("Spore Attack State")]
    [SerializeField] private MushdoomSporeAttack sporeAttack;
    [SerializeField] private float sporeAttackProbability;

    [SerializeField] private float sporeAttackDuration;
    [SerializeField] private float sporeAttackRadius;
    
    [SerializeField] private float sporeAttackTick;
    [SerializeField] private int sporeAttackTickDamage;

    private bool isSporeAttackActive = false;

    private MushdoomState currentState;

    private float aiTimer = 0f;
    private bool playerInSight = false;

    void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);

        chasingPlayerStopDistance = enemyNavMeshAgent.stoppingDistance;

        for (int i = 0; i < spinAttackColliders.Count; ++i)
        {
            spinAttackColliders[i].SetDamage(spinAttackDamage);
        }

        sporeAttack.onAttackEnd.AddListener(OnSporteAttackVolumeEnd);
        sporeAttack.SetParameters(sporeAttackDuration, sporeAttackRadius, sporeAttackTickDamage, sporeAttackTick);
    }

    public override void Reanimate()
    {
        aiTimer = 0f;
        playerInSight = false;
        hasBattleCried = false;

        isSporeAttackActive = false;
        currentState = MushdoomState.IDLE;
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
                break;

            case MushdoomState.SPORE_ATTACK:
                UpdateSporeAttackState();
                break;

            default:
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

    private void TransitionToBattleCryState()
    {
        hasBattleCried = true;
        currentState = MushdoomState.BATTLE_CRY;
        enemy.TriggerAnimation("battleCry");
    }

    public void OnBattleCryEnd()
    {
        if (currentState != MushdoomState.BATTLE_CRY)
        {
            return;
        }

        TransitionToChasingState();
    }

    private void TransitionToChasingState()
    {
        currentState = MushdoomState.CHASING_PLAYER;
        enemyNavMeshAgent.stoppingDistance = chasingPlayerStopDistance;
    }

    private void UpdateChasingPlayerState()
    {
        Vector3 playerPosition = GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition();
        enemyNavMeshAgent.SetDestination(playerPosition);
        if ((transform.position - playerPosition).sqrMagnitude <= (enemyNavMeshAgent.stoppingDistance * enemyNavMeshAgent.stoppingDistance))
        {
            float randomValue = Random.Range(0f, 1f);
            if (!isSporeAttackActive)
            {
                if (randomValue <= spinAttackProbability)
                {
                    TransitionToSpinAttack();
                }
                else if (spinAttackProbability < randomValue && randomValue <= spinAttackProbability + sporeAttackProbability)
                {
                    TransitionToSporeAttack();
                }
            }
            else
            {
                TransitionToSpinAttack();
            }
        }
    }

    private void TransitionToSpinAttack()
    {
        enemy.TriggerAnimation("spinAttack");
        currentState = MushdoomState.SPIN_ATTACK;

        for (int i = 0; i < spinAttackColliders.Count; ++i)
        {
            spinAttackColliders[i].SetColliderActive(true);
        }
    }

    private void UpdateSporeAttackState()
    {
    }
    
    public void OnSpinAttackEnd()
    {
        if (currentState != MushdoomState.SPIN_ATTACK)
        {
            throw new UnityException("OnSpinAttackEnd was captured but Mushdoom was in " + currentState + " state!");
        }

        for (int i = 0; i < spinAttackColliders.Count; ++i)
        {
            spinAttackColliders[i].SetColliderActive(false);
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
    
    private void TransitionToSporeAttack()
    {
        enemy.TriggerAnimation("sporeAttack");
        currentState = MushdoomState.SPORE_ATTACK;
    }

    public void OnSporteAttackVolumeStart()
    {
        sporeAttack.Spawn(transform.position + Vector3.up);
        isSporeAttackActive = true;
    }

    private void OnSporteAttackVolumeEnd()
    {
        isSporeAttackActive = false;
    }

    public void OnSporeAttackEnd()
    {
        if (currentState != MushdoomState.SPORE_ATTACK)
        {
            throw new UnityException("OnSporeAttackEnd was captured but Mushdoom was in " + currentState + " state!");
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

    private void OnPlayerDetected()
    {
        playerInSight = true;
        if (IsInPassiveState())
        {
            if (hasBattleCried)
            {
                TransitionToChasingState();
            }
            else
            {
                TransitionToBattleCryState();
            }
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

    public override void TransitionToDeath()
    {
        currentState = MushdoomState.DYING;
        DisableNavMeshAgent();
        enemy.TriggerAnimation("die");
    }

    public void OnDeathEnd()
    {
        if (currentState != MushdoomState.DYING)
        {
            throw new UnityException("OnDeathEnd called in state " + currentState);
        }

        enemy.Kill();
    }

    private bool IsInPassiveState()
    {
        return
            currentState == MushdoomState.IDLE
            || currentState == MushdoomState.WANDERING;
    }

    private bool IsInChasingState()
    {
        return currentState == MushdoomState.CHASING_PLAYER;
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
