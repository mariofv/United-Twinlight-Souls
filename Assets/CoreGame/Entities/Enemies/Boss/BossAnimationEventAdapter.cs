using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationEventAdapter : MonoBehaviour
{
    public UnityEvent onSlamPreparationEnd;

    public void OnSlamPreparationEnd()
    {
        onSlamPreparationEnd.Invoke();
    }
}
