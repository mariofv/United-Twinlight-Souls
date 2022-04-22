using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    private enum BossState
    {
        IDLE
    }

    private BossState currentBossState;

    private void Awake()
    {
        currentBossState = BossState.IDLE;
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

}
