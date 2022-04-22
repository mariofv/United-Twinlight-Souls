using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    private enum BossState
    {
        IDLE_PHASE_1
    }

    private BossState currentBossState = BossState.IDLE_PHASE_1;
    private int currentBossPhase = 0;
    private Transform playerTransform;

    private float currentTime = 0f;

    [Header("Boss movement")]
    [SerializeField] private Transform bossTransform;
    [SerializeField] private float maxRotationAngle;
    [SerializeField] private float rotationSpeed;

    [Header("Phase 1")]
    [SerializeField] private float minTimeBetweenAttacks;
    [SerializeField] private float maxTimeBetweenAttacks;
    private float currentTimeBetweenAttacks;

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
        }
    }

    protected override void UpdateAI()
    {
    }

    private void RotateTowardsPlayer()
    {
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f;
       
        float lookAngle = Mathf.Clamp(Vector3.SignedAngle(bossTransform.forward, lookDirection, Vector3.up), -maxRotationAngle, maxRotationAngle);
        Quaternion lookRotation = Quaternion.AngleAxis(lookAngle, Vector3.up);
        bossTransform.rotation = Quaternion.Slerp(bossTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
            
        }
    }

    public override void OnDeathStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnHitStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSpawnStart()
    {
        throw new System.NotImplementedException();
    }

    public override void Reanimate()
    {
        throw new System.NotImplementedException();
    }

    public void StartPhase(int phase)
    {
        currentBossPhase = phase;
    }

}
