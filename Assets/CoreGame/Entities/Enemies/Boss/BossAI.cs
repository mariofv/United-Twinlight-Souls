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

        TRANSITION_TO_PHASE_2,
        IDLE_PHASE_2,
        LEFT_PUNCH,
        RIGHT_PUNCH,

        TRANSITION_TO_PHASE_3,
        IDLE_PHASE_3,
        START_AVALANCHE,
        AVALANCHE,
        START_STUN,
        STUN,
    }

    private BossState currentBossState = BossState.IDLE_PHASE_1;
    private int currentBossPhase = 0;
    private Transform playerTransform;

    private float currentTime = 0f;

    [Header("Audio")]
    [SerializeField] private BossAudioClips bossAudioClips;

    [Header("Boss movement")]
    [SerializeField] private Transform bossTransform;
    [SerializeField] private float maxRotationAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Idle Phase 1")]
    [SerializeField] private float minTimeBetweenAttacksPhase1;
    [SerializeField] private float maxTimeBetweenAttacksPhase1;
    private BalancedRandomSelector phase1AttackSelector;
    private float currentTimeBetweenAttacksPhase1;

    [Header("Slam")]
    [SerializeField] private float slamPreparationTime;
    [SerializeField] private BossSlam slam;
    [SerializeField] private Transform slamHandTransform;
    [SerializeField] private float slamOffset;

    [Header("Earthquake")]
    [SerializeField] private BossEarthquake earthquake;

    [Header("Idle Phase 2")]
    [SerializeField] private float minTimeBetweenAttacksPhase2;
    [SerializeField] private float maxTimeBetweenAttacksPhase2;
    private BalancedRandomSelector phase2AttackSelector;
    private float currentTimeBetweenAttacksPhase2;

    [Header("Punches")]
    [SerializeField] private BossLeftPunch leftPunch;
    [SerializeField] private BossRightPunch rightPunch;

    [Header("Idle Phase 3")]
    [SerializeField] private List<EnemyWave> phase3Waves;
    private int currentWave = -1;

    [Header("Avalanche")]
    [SerializeField] private BossAvalanche avalanche;
    [SerializeField] private float avalancheCastingTime;
    [SerializeField] private float stunTime;

    [Space(10)]
    [SerializeField] private BossAttackRockPool bossAttackRockPool;

    private void Awake()
    {
        phase1AttackSelector = new BalancedRandomSelector(2, 0.2f);
        phase2AttackSelector = new BalancedRandomSelector(2, 0.2f);
    }

    private void Start()
    {
        playerTransform = GameManager.instance.player.GetControlledCharacter().transform;
        bossAttackRockPool.InitPool();
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

            case BossState.TRANSITION_TO_PHASE_2:
                RotateTowardsCenter();
                break;

            case BossState.IDLE_PHASE_2:
                UpdateIdlePhase2State();
                break;

            case BossState.AVALANCHE:
                UpdateAvalancheState();
                break;

            case BossState.STUN:
                UpdateStunState();
                break;
        }
    }

    protected override void UpdateAI()
    {
    }

    private void TransitionToIdlePhase1State()
    {
        currentBossState = BossState.IDLE_PHASE_1;
        currentTimeBetweenAttacksPhase1 = Random.Range(minTimeBetweenAttacksPhase1, maxTimeBetweenAttacksPhase1);
        currentTime = 0f;
    }

    private void UpdateIdlePhase1State()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= currentTimeBetweenAttacksPhase1)
        {
            int selectedAttack = phase1AttackSelector.Select();
            if (selectedAttack == 0)
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
        audioSource.PlayOneShot(bossAudioClips.slamPreparation);

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
        audioSource.PlayOneShot(bossAudioClips.slamSwing);
    }

    public void OnSlamHit()
    {
        GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.STRONG, 1f);
        audioSource.PlayOneShot(bossAudioClips.slamHit);
        slam.Trigger(slamHandTransform.position + Vector3.up * -slamOffset);
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
        audioSource.PlayOneShot(bossAudioClips.earthquakePreparation);
    }

    public void OnEarthquakeSwing()
    {
        audioSource.PlayOneShot(bossAudioClips.earthquakeSwing);
    }

    public void OnEarthquakeHit()
    {
        earthquake.Trigger();
        GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.STRONG, 2f);
        audioSource.PlayOneShot(bossAudioClips.earthquakeHit);
    }

    public void OnEarthquakeEnd()
    {
        TransitionToIdlePhase1State();
    }

    private void TransitionToTransitionToPhase2State()
    {
        currentBossState = BossState.TRANSITION_TO_PHASE_2;
        enemy.TriggerAnimation("transitionToPhase2");
    }

    public void OnTransitionToPhase2End()
    {
        TransitionToIdlePhase2State();
    }

    private void TransitionToIdlePhase2State()
    {
        currentBossState = BossState.IDLE_PHASE_2;
        currentTimeBetweenAttacksPhase2 = Random.Range(minTimeBetweenAttacksPhase2, maxTimeBetweenAttacksPhase2);
        currentTime = 0f;
    }

    private void UpdateIdlePhase2State()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= currentTimeBetweenAttacksPhase2)
        {
            int selectedAttack = phase2AttackSelector.Select();
            if (selectedAttack == 0)
            {
                TransitionToLeftPunchState();
            }
            else if (selectedAttack == 1)
            {
                TransitionToRightPunchState();
            }
        }
    }

    private void TransitionToLeftPunchState()
    {
        currentBossState = BossState.LEFT_PUNCH;
        enemy.TriggerAnimation("leftPunch");
        audioSource.PlayOneShot(bossAudioClips.punchSwing);
    }

    public void OnLeftPunchEnd()
    {
        TransitionToIdlePhase2State();
    }

    private void TransitionToRightPunchState()
    {
        currentBossState = BossState.RIGHT_PUNCH;
        enemy.TriggerAnimation("rightPunch");
        audioSource.PlayOneShot(bossAudioClips.punchSwing);

    }

    public void OnRightPunchEnd()
    {
        TransitionToIdlePhase2State();
    }

    private void TransitionToTransitionToPhase3State()
    {
        currentBossState = BossState.TRANSITION_TO_PHASE_3;
        enemy.TriggerAnimation("transitionToPhase3");
    }

    public void OnTransitionToPhase3End()
    {
        TransitionToIdlePhase3State();
    }

    private void TransitionToIdlePhase3State()
    {
        currentBossState = BossState.IDLE_PHASE_3;
        StartNextPhase3Wave();
    }

    private void StartNextPhase3Wave()
    {
        ++currentWave;
        phase3Waves[currentWave].StartWave();
        phase3Waves[currentWave].onWaveEnd.AddListener(OnWaveEnd);
    }

    private void OnWaveEnd()
    {
        phase3Waves[currentWave].onWaveEnd.RemoveAllListeners();
        if (currentWave == phase3Waves.Count - 1)
        {
            TransitionToStartAvalancheState();
        }
        else
        {
            StartNextPhase3Wave();
        }
    }

    private void TransitionToStartAvalancheState()
    {
        currentBossState = BossState.START_AVALANCHE;
        enemy.TriggerAnimation("startAvalanche");
    }

    public void OnStartAvalancheEnd()
    {
        TransitionToAvalancheState();
    }

    private void TransitionToAvalancheState()
    {
        currentBossState = BossState.AVALANCHE;
        currentTime = 0f;
        avalanche.Trigger();
    }

    private void UpdateAvalancheState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= avalancheCastingTime)
        {
            TransitionToStartStunState();
            avalanche.End();
        }
    }

    private void TransitionToStartStunState()
    {
        currentBossState = BossState.START_STUN;
        enemy.TriggerAnimation("stun");
    }

    public void OnStunStartEnd()
    {
        TransitionToStunState();
    }

    private void TransitionToStunState()
    {
        currentBossState = BossState.STUN;
        audioSource.PlayOneShotRandom(bossAudioClips.stun, bossAudioClips.stun2);
        currentTime = 0f;
    }

    private void UpdateStunState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= stunTime)
        {
            audioSource.PlayOneShot(bossAudioClips.stunRecovery);
            TransitionToStartAvalancheState();
        }
    }

    public override void OnDeathStart()
    {
        GameManager.instance.levelManager.GetCurrentLevelAsBoss().TriggerBossDeath();
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
        currentBossPhase = phase;
        switch (currentBossPhase)
        {
            case 0:
                enemy.Reanimate();
                break;

            case 1:
                TransitionToTransitionToPhase2State();
                break;

            case 2:
                TransitionToTransitionToPhase3State();
                break;

            default:
                break;
        }
    }

    public void OnRoarStart()
    {
        GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.STRONG, 2f);
        audioSource.PlayOneShotRandom(bossAudioClips.roar, bossAudioClips.roarKingKong);
    }

    public void OnPunchHit()
    {
        audioSource.PlayOneShot(bossAudioClips.punchHit);
        if (currentBossState == BossState.LEFT_PUNCH)
        {
            GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.NORMAL, 2f);
            leftPunch.Trigger();

        }
        else if (currentBossState == BossState.RIGHT_PUNCH)
        {
            GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.NORMAL, 2f);
            rightPunch.Trigger();
        }
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
