using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationEventAdapter : MonoBehaviour
{
    [SerializeField] private BossAI bossAI;

    private UnityEvent onBossSlamPreparationEnd = new UnityEvent();
    private UnityEvent onBossSlamEnd = new UnityEvent();
    private UnityEvent onBossSlamRestEnd = new UnityEvent();
    private UnityEvent onBossSlamRecoveryEnd = new UnityEvent();

    private UnityEvent onEarthquakeHit = new UnityEvent();
    private UnityEvent onEarthquakeEnd = new UnityEvent();

    private UnityEvent onTransitionToPhase2End = new UnityEvent();

    private void Awake()
    {
        onBossSlamPreparationEnd.AddListener(bossAI.OnSlamPreparationEnd);
        onBossSlamEnd.AddListener(bossAI.OnSlamEnd);
        onBossSlamRestEnd.AddListener(bossAI.OnSlamRestEnd);
        onBossSlamRecoveryEnd.AddListener(bossAI.OnSlamRecoveryEnd);

        onEarthquakeHit.AddListener(bossAI.OnEarthquakeHit);
        onEarthquakeEnd.AddListener(bossAI.OnEarthquakeEnd);

        onTransitionToPhase2End.AddListener(bossAI.OnTransitionToPhase2End);
    }

    public void OnBossSlamPreparationEnd()
    {
        onBossSlamPreparationEnd.Invoke();
    }

    public void OnBossSlamEnd()
    {
        onBossSlamEnd.Invoke();
    }

    public void OnBossSlamRestEnd()
    {
        onBossSlamRestEnd.Invoke();
    }

    public void OnBossSlamRecoveryEnd()
    {
        onBossSlamRecoveryEnd.Invoke();
    }

    public void OnEarthquakeHit()
    {
        onEarthquakeHit.Invoke();
    }

    public void OnEarthquakeEnd()
    {
        onEarthquakeEnd.Invoke();
    }

    public void OnTransitionToPhase2End()
    {
        onTransitionToPhase2End.Invoke();
    }
}
