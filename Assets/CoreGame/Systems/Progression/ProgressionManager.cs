using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [System.Flags]
    public enum Progression
    {
        NONE = 0,
        BASIC_COMBAT_UNLOCKED = 1,
        DEFENSIVE_COMBAT_UNLOCKED = 2,
        SPECIAL_ATTACK_UNLOCKED = 4,
    };

    private Progression currentProgression;
    private int maxLevelUnlocked = 0;

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
            UnlockEverything();
        }
    }

    public void UnlockEverything()
    {
        UnlockProgression(Progression.BASIC_COMBAT_UNLOCKED);
        UnlockProgression(Progression.SPECIAL_ATTACK_UNLOCKED);
        UnlockProgression(Progression.DEFENSIVE_COMBAT_UNLOCKED);
    }
}
