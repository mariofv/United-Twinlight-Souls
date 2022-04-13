using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NecroplantRootsAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onSpawnEnd;
    public UnityEvent onBurrowEnd;

    public void OnSpawnEnd()
    {
        onSpawnEnd.Invoke();
    }

    public void OnBurrowEnd()
    {
        onBurrowEnd.Invoke();
    }
}
