using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationEventsManager : CharacterSubManager
{
    public UnityEvent onDeathEnd;
    public UnityEvent onLightAttackEnd;

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }

    public void OnLightAttackEnd()
    {
        onLightAttackEnd.Invoke();
    }
}
