using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MushdoomAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onSporeAttackVolumeStart;
    public UnityEvent onSporeAttackEnd;
    public UnityEvent onSpinAttackEnd;

    public UnityEvent onBattleCryEnd;
    public UnityEvent onHitEnd;
    public UnityEvent onDeathEnd;

    public void OnSporeAttackVolumeStart()
    {
        onSporeAttackVolumeStart.Invoke();
    }

    public void OnSporeAttackEnd()
    {
        onSporeAttackEnd.Invoke();
    }

    public void OnSpinAttackEnd()
    {
        onSpinAttackEnd.Invoke();
    }

    public void OnBattleCryEnd()
    {
        onBattleCryEnd.Invoke();
    }

    public void OnHitEnd()
    {
        onHitEnd.Invoke();
    }

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }
}
