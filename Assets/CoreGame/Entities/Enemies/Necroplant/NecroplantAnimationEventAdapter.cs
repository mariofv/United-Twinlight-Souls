using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NecroplantAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onDeathEnd;

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }
}
