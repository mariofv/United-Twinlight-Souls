using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : Pickup
{
    [SerializeField] private int healedAmount;

    protected override void ApplyEffect()
    {
        GameManager.instance.player.GetControlledCharacter().characterStatsManager.Heal(healedAmount);
    }
}
