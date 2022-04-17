using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiterAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onHitEnd;
    public UnityEvent onDeathEnd;
    public UnityEvent onBiteAttackEnd;
    public UnityEvent onDashAttackStart;
    public UnityEvent onDashAttackEnd;

    public void OnHitEnd()
    {
        onHitEnd.Invoke();
    }

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }

    public void OnBiteAttackEnd()
    {
        onBiteAttackEnd.Invoke();
    }

    public void OnDashAttackStart()
    {
        onDashAttackStart.Invoke();
    }

    public void OnDashAttackEnd()
    {
        onDashAttackEnd.Invoke();
    }
}
