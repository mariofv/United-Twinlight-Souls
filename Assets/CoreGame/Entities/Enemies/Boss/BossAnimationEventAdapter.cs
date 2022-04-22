using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onSlamPreparationEnd;
    public UnityEvent onBossSlamEnd;

    public void OnSlamPreparationEnd()
    {
        onSlamPreparationEnd.Invoke();
    }

    public void OnBossSlamEnd()
    {
        onBossSlamEnd.Invoke();
    }
}
