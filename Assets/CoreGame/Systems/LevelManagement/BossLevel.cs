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

    public void ForceStartBossPhase(int bossPhase)
    {
        Enemy bossScript = GameObject.Find("Boss").GetComponent<Enemy>();
        int bossCurrentHealth = bossScript.GetCurrentHealth();
        int bossMaxHealth = bossScript.GetMaxHealth();
        float phaseHealthPercentage = phasesThresholds[bossPhase];

        float necessaryDamage = bossCurrentHealth - phaseHealthPercentage * bossMaxHealth + 10;
        bossScript.Hurt(Mathf.RoundToInt(necessaryDamage));
    }

    public void CheckPhaseTransition(float healthProgress)
    {
        int newPhase = -1;
        for (int i = 0; i < phasesThresholds.Count; ++i)
        {
            if (phasesThresholds[i] >= healthProgress)
            {
                ++newPhase;
            }
            else
            {
                break;
            }
        }

        if (currentBossPhase != newPhase)
        {
            currentBossPhase = newPhase;
            StartBossPhase(currentBossPhase);
        }
    }

    public int GetNumberOfPhases()
    {
        return phasesThresholds.Count;
    }

    public override CinemachineVirtualCamera GetCurrentCamera()
    {
        return virtualCamera;
    }
}
