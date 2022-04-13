using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiterAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onHitEnd;
    public UnityEvent onDeathEnd;

    public void OnHitEnd()
    {
        onHitEnd.Invoke();
    }

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }
}
