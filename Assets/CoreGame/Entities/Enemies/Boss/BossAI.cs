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

    [Header("Boss Audio")]
    [SerializeField] private BossAudioAdapter bossAudioAdapter;

    [Header("Boss movement")]
    [SerializeField] private Transform bossTransform;
    [SerializeField] private float maxRotationAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Idle Phase 1")]
    [SerializeField] private float minTimeBetweenAttacksPhase1;
    [SerializeField] private float maxTimeBetweenAttacksPhase1;
    [SerializeField] private BossAttacksSelector phase1AttackSelector;
    private float currentTimeBetweenAttacksPhase1;

    [Header("Slam")]
    [SerializeField] private float slamPreparationTime;

    [Header("Idle Phase 2")]
    [SerializeField] private float minTimeBetweenAttacksPhase2;
    [SerializeField] private float maxTimeBetweenAttacksPhase2;
    [SerializeField] private BossAttacksSelector phase2AttackSelector;
    private float currentTimeBetweenAttacksPhase2;

    [Header("Idle Phase 3")]
    [SerializeField] private List<EnemyWave> phase3Waves;
    [SerializeField] private float avalancheCastingTime;
    [SerializeField] private float stunTime;
    private int currentWave = -1;

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
            int selectedAttack = phase1AttackSelector.SelectAttack();
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
        GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.STRONG, 1f);
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
        GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.STRONG, 2f);
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
            int selectedAttack = phase2AttackSelector.SelectAttack();
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

    }

    public void OnLeftPunchEnd()
    {
        TransitionToIdlePhase2State();
    }

    private void TransitionToRightPunchState()
    {
        currentBossState = BossState.RIGHT_PUNCH;
        enemy.TriggerAnimation("rightPunch");

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
    }

    private void UpdateAvalancheState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= avalancheCastingTime)
        {
            TransitionToStartStunState();
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
        bossAudioAdapter.stun.Play();

        currentTime = 0f;
    }

    private void UpdateStunState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= stunTime)
        {
            TransitionToStartAvalancheState();
        }
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
        float random = Random.Range(0f, 1f);
        if (random >= 0.5f)
        {
            bossAudioAdapter.roar.Play();
        }
        else
        {
            bossAudioAdapter.roarKingKong.Play();
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
