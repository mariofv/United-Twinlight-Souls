using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationEventsManager : CharacterSubManager
{
    public UnityEvent onDeathEnd;
    
    public UnityEvent onLightAttackEnd;
    public UnityEvent onLightAttackEnableHitbox;
    public UnityEvent onLightAttackDisableHitbox;

    public UnityEvent onSpecialAttackThrow;
    public UnityEvent onSpecialAttackEnd;

    public void OnDeathEnd()
    {
        onDeathEnd.Invoke();
    }

    public void OnLightAttackEnd()
    {
        onLightAttackEnd.Invoke();
    }

    public void OnLightAttackEnableHitbox()
    {
        onLightAttackEnableHitbox.Invoke();
    }

    public void OnLightAttackDisableHitbox()
    {
        onLightAttackDisableHitbox.Invoke();
    }

    public void OnSpecialAttackThrow()
    {
        onSpecialAttackThrow.Invoke();
    }

    public void OnSpecialAttackEnd()
    {
        onSpecialAttackEnd.Invoke();
    }
}
