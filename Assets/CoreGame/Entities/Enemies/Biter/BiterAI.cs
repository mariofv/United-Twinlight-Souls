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
        DASHING,
        EXPLODING,
        HIT,
        DYING
    }

    private BiterState currentState;
    private bool playerInSight = false;

    [Header("Audio")]
    [SerializeField] private BiterAudioClips biterAudioClips;

    [Header("Floating")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float floatingAmplitude;

    [Header("Chasing State")]
    private float originalHeight;
    private float chasingPlayerStopDistance;

    [Header("Biting State")]
    [SerializeField] private float biteAttackCooldown;
    private float timeToNextBiteAttack;
    [SerializeField] private int biteAttackDamage;
    [SerializeField] private BiterBiteAttack biterBiteAttack;

    [Header("Dashing State")]
    [SerializeField] private float dashAttackCooldown;
    private float timeToNextDashAttack;
    [SerializeField] private int dashAttackDamage;
    [SerializeField] private BiterDashAttack biterDashAttack;
    [SerializeField] private float distanceToTriggerDashAttack;
    [SerializeField] private float dashAttackSpeed;
    private Vector3 dashAttackDirection;
    private bool dashing = false;

    [Header("Explosion State")]
    [SerializeField] [Range(0f, 1f)] private float explosionHealthThreshold;
    [SerializeField] private int explosionAttackDamage;
    [SerializeField] private BiterExplosionAttack biterExplosionAttack;

    private void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);

        biterBiteAttack.SetDamage(biteAttackDamage);
        biterDashAttack.SetDamage(dashAttackDamage);
        biterExplosionAttack.SetDamage(explosionAttackDamage);
        biterExplosionAttack.onExplosionEndEvent.AddListener(OnExplosionEnd);

        chasingPlayerStopDistance = enemyNavMeshAgent.stoppingDistance;
    }

    public override void Reanimate()
    {
        playerInSight = false;
        currentState = BiterState.IDLE;
        originalHeight = bodyTransform.position.y;

        timeToNextBiteAttack = 0f;
        timeToNextDashAttack = 0f;
        dashing = false;
    }

    protected override void UpdateSpecific()
    {
        switch (currentState)
        {
            case BiterState.SPAWN:
                break;
            case BiterState.IDLE:
                UpdateState();
                break;
            case BiterState.CHASING_PLAYER:
                UpdateState();
                break;
            case BiterState.BITING:
                UpdateState();
                break;
            case BiterState.DASHING:
                UpdateState();
                UpdateDasingAttackState();
                break;
            case BiterState.HIT:
                UpdateState();
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
            case BiterState.DASHING:
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

    private void UpdateState()
    {
        UpdateCooldowns();
        UpdateFloatingPosition();
    }

    private void UpdateCooldowns()
    {
        if (timeToNextBiteAttack > 0)
        {
            timeToNextBiteAttack = Mathf.Max(0f, timeToNextBiteAttack - Time.deltaTime);
        }
        if (timeToNextDashAttack > 0)
        {
            timeToNextDashAttack = Mathf.Max(0f, timeToNextDashAttack - Time.deltaTime);
        }
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

        float sqrDistanceToPlayer = (transform.position - playerPosition).sqrMagnitude;
        if (sqrDistanceToPlayer <= (distanceToTriggerDashAttack * distanceToTriggerDashAttack) && timeToNextDashAttack == 0)
        {
            dashAttackDirection = (playerPosition - transform.position).normalized;
            enemyNavMeshAgent.ResetPath();
            TransitionToDashState();
        }
        else if (sqrDistanceToPlayer <= (enemyNavMeshAgent.stoppingDistance * enemyNavMeshAgent.stoppingDistance))
        {
            if (enemy.GetCurrentHealthPercentage() <= explosionHealthThreshold)
            {
                TransitionToExplodingState();
            }
            else
            {
                if (timeToNextBiteAttack == 0)
                {
                    TransitionToBiteState();
                }
            }
        }
    }

    private void TransitionToBiteState()
    {
        enemy.TriggerAnimation("bite");
        currentState = BiterState.BITING;
        biterBiteAttack.SetColliderActive(true);
        audioSource.PlayOneShotRandom(
            biterAudioClips.bite1,
            biterAudioClips.bite2,
            biterAudioClips.bite3,
            biterAudioClips.bite4
        );
    }

    public void OnBiteAttackEnd()
    {
        biterBiteAttack.SetColliderActive(false);
        timeToNextBiteAttack = biteAttackCooldown;

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

    private void TransitionToDashState()
    {
        enemy.TriggerAnimation("dash");
        currentState = BiterState.DASHING;
    }

    public void OnActualDashStart()
    {
        dashing = true;
        biterDashAttack.SetColliderActive(true);
    }

    private void UpdateDasingAttackState()
    {
        if (dashing)
        {
            enemyNavMeshAgent.Move(dashAttackDirection * dashAttackSpeed * Time.deltaTime);
        }
    }

    public void OnDashAttackBite()
    {
        audioSource.PlayOneShotRandom(
            biterAudioClips.bite1,
            biterAudioClips.bite2,
            biterAudioClips.bite3,
            biterAudioClips.bite4
        );
    }

    public void OnDashAttackEnd()
    {
        biterDashAttack.SetColliderActive(false);
        timeToNextDashAttack = dashAttackCooldown;
        dashing = false;

        if (currentState != BiterState.DASHING)
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

    private void TransitionToExplodingState()
    {
        enemy.SetInvincible(true);
        enemy.TriggerAnimation("explode");
        currentState = BiterState.EXPLODING;
        biterExplosionAttack.OnExplosionStart();
    }

    private void OnExplosionEnd()
    {
        OnDeathStart();
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
        audioSource.PlayOneShotRandom(
            biterAudioClips.death1,
            biterAudioClips.death2
        );
    }

    public void OnDeathEnd()
    {
        if (currentState != BiterState.DYING)
        {
            throw new UnityException("OnDeathEnd called in state " + currentState);
        }

        enemy.Kill();
    }

    private void OnPlayerDetected(Transform detectedPlayer)
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
