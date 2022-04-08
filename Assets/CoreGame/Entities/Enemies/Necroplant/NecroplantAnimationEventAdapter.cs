using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NecroplantAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onShoot;
    public UnityEvent onShootAnimationEnd;
    public UnityEvent onDeathEnd;

    public void OnShoot()
    {
        onShoot.Invoke();
    }

    public void OnShootAnimationEnd()
    {
        onShootAnimationEnd.Invoke();
    }

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }
}
