using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [System.Flags]
    public enum Progression
    {
        NONE = 0,
        LIGHT_ATTACK_UNLOCKED = 1,
        LOCK_ON_UNLOCKED = 2,
        SHIELD_UNLOCKED = 4,
        DASH_UNLOCKED = 8,
    };

    private Progression currentProgression;

    public bool CheckProgression(Progression progression)
    {
        return (currentProgression & progression) == progression;
    }

    public void UnlockProgression(Progression progression)
    {
        currentProgression |= progression;
    }
}
