using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject damageIndicatorPrefab;
    private SettingsManager settingsManager;

    private void Awake()
    {
        settingsManager = GameManager.instance.settingsManager;
    }

    public void ShowDamageIndicator(int damage, Vector3 damageInflicterPosition, Vector3 damageReceiverPosition)
    {
        if (!settingsManager.damageIndicatorEnabled)
        {
            return;
        }

        DamageIndicator spawnedDamagedIndicator = Instantiate(damageIndicatorPrefab, transform).GetComponent<DamageIndicator>();
        spawnedDamagedIndicator.Spawn(damage, damageInflicterPosition, damageReceiverPosition);
    }
}
