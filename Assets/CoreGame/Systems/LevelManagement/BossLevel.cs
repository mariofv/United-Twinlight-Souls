using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : Level
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private BossAI bossAI;

    private int currentBossPhase = 0;

    private void Start()
    {
        StartBossPhase(0);
    }

    private void StartBossPhase(int bossPhase)
    {
        bossAI.StartPhase(bossPhase);
    }

    public override CinemachineVirtualCamera GetCurrentCamera()
    {
        return virtualCamera;
    }


}
