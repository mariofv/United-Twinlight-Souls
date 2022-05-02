using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    private enum BossState
    {
        IDLE_PHASE_1,
        SLAM_PREPARATION,
        SLAM,
        SLAM_REST,
        SLAM_RECOVERY,
        EARTHQUAKE,
    }

    private BossState currentBossState = BossState.IDLE_PHASE_1;
    private int currentBossPhase = 0;
    private Transform playerTransform;

    private float currentTime = 0f;

    [Header("Boss movement")]
    [SerializeField] private Transform bossTransform;
    [SerializeField] private float maxRotationAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Idle Phase 1")]
    [SerializeField] private float minTimeBetweenAttacks;
    [SerializeField] private float maxTimeBetweenAttacks;
    private float currentTimeBetweenAttacks;

    [Header("Slam")]
    [SerializeField] private float slamPreparationTime;

    private void Awake()
    {
    }

    private void Start()
    {
        playerTransform = GameManager.instance.player.GetControlledCharacter().transform;
        TransitionToIdlePhase1State();
    }

    protected override void UpdateSpecific()
    {
        switch (currentBossState)
        {
            case BossState.IDLE_PHASE_1:
                RotateTowardsPlayer();
                UpdateIdlePhase1State();
                break;

            case BossState.SLAM_PREPARATION:
                RotateTowardsPlayer();
                UpdateSlamPreparationState();
                break;

            case BossState.EARTHQUAKE:
                RotateTowardsCenter();
                break;
        }
    }

    protected override void UpdateAI()
    {
    }

    private void TransitionToIdlePhase1State()
    {
        currentBossState = BossState.IDLE_PHASE_1;
        currentTimeBetweenAttacks = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        currentTime = 0f;
    }

    private void UpdateIdlePhase1State()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= currentTimeBetweenAttacks)
        {
            float random = Random.Range(0f, 1f);
            if (random >= 0.5f)
            {
                TransitionToSlamPreparationState();
            }
            else
            {
                TransitionToEarthquakeState();
            }
        }
    }

    private void TransitionToSlamPreparationState()
    {
        currentBossState = BossState.SLAM_PREPARATION;
        enemy.TriggerAnimation("prepareSlam");
        currentTime = 0f;
    }

    private void UpdateSlamPreparationState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= slamPreparationTime)
        {
            TransitionToSlamState();
        }
    }

    public void OnSlamPreparationEnd()
    {
        enemy.SetAnimatorSpeed(0f);
    }

    private void TransitionToSlamState()
    {
        currentBossState = BossState.SLAM;
        enemy.SetAnimatorSpeed(1f);
        enemy.TriggerAnimation("slam");
    }

    public void OnSlamEnd()
    {
        TransitionToSlamRestState();
    }

    private void TransitionToSlamRestState()
    {
        currentBossState = BossState.SLAM_REST;
    }

    public void OnSlamRestEnd()
    {
        TransitionToSlamRecoveryState();
    }

    private void TransitionToSlamRecoveryState()
    {
        currentBossState = BossState.SLAM_RECOVERY;
        enemy.TriggerAnimation("slamRecovery");
    }

    public void OnSlamRecoveryEnd()
    {
        TransitionToIdlePhase1State();
    }

    private void TransitionToEarthquakeState()
    {
        currentBossState = BossState.EARTHQUAKE;
        enemy.TriggerAnimation("earthquake");
    }

    public void OnEarthquakeHit()
    {
    }

    public void OnEarthquakeEnd()
    {
        TransitionToIdlePhase1State();
    }

    public override void OnDeathStart()
    {
        return;
    }

    public override void OnHitStart()
    {

    }

    public override void OnSpawnStart()
    {

    }

    public override void Reanimate()
    {

    }

    public void StartPhase(int phase)
    {
        if (phase == 0)
        {
            enemy.Reanimate();
        }
        currentBossPhase = phase;
    }

    private void RotateTowardsCenter()
    {
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward);
        RotateTowardsQuaternion(lookRotation);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f;

        float lookAngle = Mathf.Clamp(Vector3.SignedAngle(bossTransform.forward, lookDirection, Vector3.up), -maxRotationAngle, maxRotationAngle);
        Quaternion lookRotation = Quaternion.AngleAxis(lookAngle, Vector3.up);
        RotateTowardsQuaternion(lookRotation);
    }

    private void RotateTowardsQuaternion(Quaternion rotation)
    {
        bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
