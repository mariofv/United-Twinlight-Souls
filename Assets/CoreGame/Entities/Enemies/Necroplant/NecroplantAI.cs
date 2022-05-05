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

    [Header("Audio")]
    [SerializeField] private AudioSource burroAudioSource;
    [SerializeField] private NecroplantAudioClips necroplantAudioClips;

    [Header("Spawn State")]
    [SerializeField] private Transform necroplantBodyTransform;
    private float spawnTime;

    [Header("Aiming State")]
    [SerializeField] private Transform necroplantAimingBodyTransform;
    [SerializeField] private float aimingRotationSpeed;
    [SerializeField] private float maxAimingTime;
    private Transform targetedPlayerTransform;

    [Header("Shooting State")]
    [SerializeField] private NecroMuzzle necroplantNecroMuzzle;
    [SerializeField] private int numberOfShootsBeforeSpecialAttack;
    [SerializeField] private float shotgunAttackAngle;
    private int currentNumberOfShoots = 0;

    [Header("Burrowing State")]
    [SerializeField] private float distanceToBurrowAway;
    [SerializeField] private float playerDistanceToBorrow;
    [SerializeField] private float minAimingTimeToBurrow;
    [SerializeField] private float burrowingCooldown;
    private Vector3 burrowTargetPosition;
    private float timeSinceLastBurrow = 0f;

    private void Awake()
    {
        playerDetectionCollider.onPlayerDetected.AddListener(OnPlayerDetected);
        playerDetectionCollider.onPlayerLost.AddListener(OnPlayerLost);

        spawnTime = 1.63f;
    }

    public override void Reanimate()
    {
        aiTimer = 0f; 
        playerInSight = false; 
        timeSinceLastBurrow = burrowingCooldown;
        currentNumberOfShoots = 0;
        currentState = NecroplantState.IDLE;
    }

    protected override void UpdateSpecific()
    {
        timeSinceLastBurrow += Time.deltaTime;
        switch (currentState)
        {
            case NecroplantState.SPAWN:
                UpdateSpawnState();
                break;
            case NecroplantState.TELEPORT_OUT:
                UpdateBurrowState();
                break;
            case NecroplantState.IDLE:
                break;
            case NecroplantState.AIMING_PLAYER:
                UpdateAimingPlayerState();
                break;
            case NecroplantState.SIMPLE_ATTACK:
                UpdateShootingState();
                break;
            case NecroplantState.SHOTGUN_ATTACK:
                UpdateShootingState();
                break;
            case NecroplantState.BARRAGE_ATTACK:
                UpdateShootingState();
                break;
            case NecroplantState.DYING:
                break;
        }
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
                UpdateAimingAIPlayerState();
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
        rootsAnimatorController.SetTrigger("spawn");
        enemy.SetInvincible(true);
        necroplantBodyTransform.localScale = Vector3.zero;
        aiTimer = 0f;

        audioSource.PlayOneShot(necroplantAudioClips.taunt);
        burroAudioSource.Play();
    }

    private void UpdateSpawnState()
    {
        aiTimer += Time.deltaTime;
        float progress = Mathf.Min(1f, aiTimer / spawnTime);

        necroplantBodyTransform.localScale = Vector3.one * progress;
    }

    public void OnSpawnEnd()
    {
        if (currentState != NecroplantState.SPAWN)
        {
            throw new UnityException("OnSpawnEnd was captured but Necroplant was in " + currentState + " state!");
        }

        burroAudioSource.Stop();

        enemy.SetInvincible(false);
        necroplantBodyTransform.localScale = Vector3.one;
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
        targetedPlayerTransform = GameManager.instance.player.GetControlledCharacter().transform;
    }

    private void UpdateAimingPlayerState()
    {
        Vector3 lookDirection = targetedPlayerTransform.position - transform.position;
        lookDirection.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        necroplantAimingBodyTransform.rotation = Quaternion.Slerp(necroplantAimingBodyTransform.rotation, lookRotation, Time.deltaTime * aimingRotationSpeed);
    }

    private void UpdateAimingAIPlayerState()
    {
        aiTimer += deltaTimeAI;
        if (aiTimer >= minAimingTimeToBurrow && timeSinceLastBurrow >= burrowingCooldown)
        {
            float sqrDistanceToPlayer = (targetedPlayerTransform.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < (playerDistanceToBorrow * playerDistanceToBorrow))
            {
                TransitionToBurrowState();
            }
        }
        if (aiTimer >= maxAimingTime)
        {
            if (currentNumberOfShoots < numberOfShootsBeforeSpecialAttack)
            {
                currentState = NecroplantState.SIMPLE_ATTACK;
            }
            else
            {
                float random = Random.Range(0f, 1f);
                if (random < 0.5f)
                {
                    currentState = NecroplantState.SHOTGUN_ATTACK;
                }
                else
                {
                    currentState = NecroplantState.BARRAGE_ATTACK;
                }
                currentNumberOfShoots = 0;
            }
            enemy.TriggerAnimation("shoot");
        }
    }

    private void TransitionToBurrowState()
    {
        currentState = NecroplantState.TELEPORT_OUT;
        rootsAnimatorController.SetTrigger("burrow");
        enemy.SetInvincible(true);
        aiTimer = 0f;
        burrowTargetPosition = NavMeshHelper.FindAvailableSpawnPosition(transform.position, distanceToBurrowAway);
        burroAudioSource.Play();
    }

    private void UpdateBurrowState()
    {
        aiTimer += Time.deltaTime;
        float progress = Mathf.Min(1f, aiTimer / spawnTime);

        necroplantBodyTransform.localScale = Vector3.one * (1f - progress);
    }

    public void OnBurrowEnd()
    {
        if (currentState != NecroplantState.TELEPORT_OUT)
        {
            throw new UnityException("OnBurrowEnd was captured but Necroplant was in " + currentState + " state!");
        }

        burroAudioSource.Stop();

        timeSinceLastBurrow = 0f;
        necroplantBodyTransform.localScale = Vector3.zero;
        enemyNavMeshAgent.Warp(burrowTargetPosition);
        OnSpawnStart();
    }

    private void UpdateShootingState()
    {
        Vector3 lookDirection = targetedPlayerTransform.position - transform.position;
        lookDirection.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        necroplantAimingBodyTransform.rotation = Quaternion.Slerp(necroplantAimingBodyTransform.rotation, lookRotation, Time.deltaTime * aimingRotationSpeed);
    }

    public void OnShoot()
    {
        Vector3 directionToPlayer = (targetedPlayerTransform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        audioSource.PlayOneShotRandom(necroplantAudioClips.shoot1, necroplantAudioClips.shoot2);
        if (currentState == NecroplantState.SIMPLE_ATTACK || currentState == NecroplantState.BARRAGE_ATTACK)
        {
            necroplantNecroMuzzle.Shoot(directionToPlayer);
            ++currentNumberOfShoots;
        }
        else if (currentState == NecroplantState.SHOTGUN_ATTACK)
        {
            necroplantNecroMuzzle.Shoot(directionToPlayer);

            Vector3 coneLeftSide = Quaternion.Euler(0f, -shotgunAttackAngle, 0f) * directionToPlayer;
            necroplantNecroMuzzle.Shoot(coneLeftSide);

            Vector3 coneRightSide = Quaternion.Euler(0f, shotgunAttackAngle, 0f) * directionToPlayer;
            necroplantNecroMuzzle.Shoot(coneRightSide);
        }
    }

    public void OnShootingAnimationEnd()
    {
        if (currentState == NecroplantState.SIMPLE_ATTACK || currentState == NecroplantState.SHOTGUN_ATTACK)
        {
            if (!playerInSight)
            {
                TransitionToIdleState();
            }
            else
            {
                TransitionToAimingState();
            }
        }
        else if (currentState == NecroplantState.BARRAGE_ATTACK)
        {
            if (currentNumberOfShoots < 3)
            {
                enemy.TriggerAnimation("shoot");
            }
            else
            {
                currentNumberOfShoots = 0;
                if (!playerInSight)
                {
                    TransitionToIdleState();
                }
                else
                {
                    TransitionToAimingState();
                }
            }
        }
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
        audioSource.PlayOneShotRandom(necroplantAudioClips.death1, necroplantAudioClips.death2, necroplantAudioClips.death3);
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
