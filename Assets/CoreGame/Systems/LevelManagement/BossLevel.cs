using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : Level
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private List<float> phasesThresholds;

    private int currentBossPhase = 0;

    private void Start()
    {
        StartBossPhase(0);
    }

    private void StartBossPhase(int bossPhase)
    {
        bossAI.StartPhase(bossPhase);
    }

    public void CheckPhaseTransition(float healthProgress)
    {
        if (phasesThresholds[currentBossPhase] >= healthProgress)
        {
            ++currentBossPhase;
            StartBossPhase(currentBossPhase);
        }
    }

    public override CinemachineVirtualCamera GetCurrentCamera()
    {
        return virtualCamera;
    }


}
