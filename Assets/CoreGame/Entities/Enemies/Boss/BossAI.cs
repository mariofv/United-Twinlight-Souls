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

    private void Awake()
    {
    }

    protected override void UpdateAI()
    {
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
