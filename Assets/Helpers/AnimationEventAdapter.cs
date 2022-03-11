using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventAdapter : MonoBehaviour
{
    public List<UnityEvent> animationEvents;

    // Update is called once per frame
    void Update()
    {
    }

    public void OnAnimationEvent(int i)
    {
        animationEvents[i].Invoke();
    }
}
