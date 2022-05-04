using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationEventAdapter : MonoBehaviour
{
    [SerializeField] private BossAI bossAI;

    private UnityEvent onBossSlamPreparationEnd = new UnityEvent();
    private UnityEvent onBossSlamHit = new UnityEvent();
    private UnityEvent onBossSlamEnd = new UnityEvent();
    private UnityEvent onBossSlamRestEnd = new UnityEvent();
    private UnityEvent onBossSlamRecoveryEnd = new UnityEvent();

    private UnityEvent onEarthquakeHit = new UnityEvent();
    private UnityEvent onEarthquakeEnd = new UnityEvent();

    private UnityEvent onTransitionToPhase2End = new UnityEvent();
    private UnityEvent onLeftPunchEnd = new UnityEvent();
    private UnityEvent onRightPunchEnd = new UnityEvent();

    private UnityEvent onTransitionToPhase3End = new UnityEvent();
    private UnityEvent onStartAvalancheEnd = new UnityEvent();
    private UnityEvent onStunStartEnd = new UnityEvent();

    private UnityEvent onRoarStart = new UnityEvent();

    private void Awake()
    {
        onBossSlamPreparationEnd.AddListener(bossAI.OnSlamPreparationEnd);
        onBossSlamHit.AddListener(bossAI.OnSlamHit);
        onBossSlamEnd.AddListener(bossAI.OnSlamEnd);
        onBossSlamRestEnd.AddListener(bossAI.OnSlamRestEnd);
        onBossSlamRecoveryEnd.AddListener(bossAI.OnSlamRecoveryEnd);

        onEarthquakeHit.AddListener(bossAI.OnEarthquakeHit);
        onEarthquakeEnd.AddListener(bossAI.OnEarthquakeEnd);

        onTransitionToPhase2End.AddListener(bossAI.OnTransitionToPhase2End);
        onLeftPunchEnd.AddListener(bossAI.OnLeftPunchEnd);
        onRightPunchEnd.AddListener(bossAI.OnRightPunchEnd);

        onTransitionToPhase3End.AddListener(bossAI.OnTransitionToPhase3End);
        onStartAvalancheEnd.AddListener(bossAI.OnStartAvalancheEnd);
        onStunStartEnd.AddListener(bossAI.OnStunStartEnd);

        onRoarStart.AddListener(bossAI.OnRoarStart);
    }

    public void OnBossSlamPreparationEnd()
    {
        onBossSlamPreparationEnd.Invoke();
    }

    public void OnBossSlamHit()
    {
        onBossSlamHit.Invoke();
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

    public void OnLeftPunchEnd()
    {
        onLeftPunchEnd.Invoke();
    }

    public void OnRightPunchEnd()
    {
        onRightPunchEnd.Invoke();
    }

    public void OnTransitionToPhase3End()
    {
        onTransitionToPhase3End.Invoke();
    }

    public void OnStartAvalancheEnd()
    {
        onStartAvalancheEnd.Invoke();
    }

    public void OnStunStartEnd()
    {
        onStunStartEnd.Invoke();
    }

    public void OnRoarStart()
    {
        onRoarStart.Invoke();
    }
}
