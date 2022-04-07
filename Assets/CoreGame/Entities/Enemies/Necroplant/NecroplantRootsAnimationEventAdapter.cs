using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NecroplantRootsAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onSpawnEnd;
    public UnityEvent onTeleportOutEnd;

    public void OnSpawnEnd()
    {
        onSpawnEnd.Invoke();
    }

    public void OnTeleportOutEnd()
    {
        onTeleportOutEnd.Invoke();
    }
}
