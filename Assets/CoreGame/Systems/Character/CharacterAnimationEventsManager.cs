using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationEventsManager : CharacterSubManager
{
    public UnityEvent onLightAttackEnd;

    public void OnLightAttackEnd()
    {
        onLightAttackEnd.Invoke();
    }
}
