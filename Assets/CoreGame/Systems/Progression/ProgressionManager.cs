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
    private int maxLevelUnlocked = 2;

    public bool CheckProgression(Progression progression)
    {
        return (currentProgression & progression) == progression;
    }

    public void UnlockProgression(Progression progression)
    {
        currentProgression |= progression;
    }

    public int GetMaxLevelUnlocked()
    {
        return maxLevelUnlocked;
    }

    public void SetMaxLevelUnlocked(int level)
    {
        maxLevelUnlocked = Mathf.Max(maxLevelUnlocked, level);
        if (maxLevelUnlocked != 0)
        {
            UnlockProgression(Progression.LIGHT_ATTACK_UNLOCKED);
            UnlockProgression(Progression.LOCK_ON_UNLOCKED);
            UnlockProgression(Progression.SHIELD_UNLOCKED);
            UnlockProgression(Progression.DASH_UNLOCKED);
        }
    }
}
