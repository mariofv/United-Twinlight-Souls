using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onBossSlamPreparationEnd;
    public UnityEvent onBossSlamEnd;
    public UnityEvent onBossSlamRestEnd;
    public UnityEvent onBossSlamRecoveryEnd;

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
}
