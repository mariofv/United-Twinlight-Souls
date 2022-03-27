using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject damageIndicatorPrefab;

    public void ShowDamageIndicator(int damage, Vector3 damageInflicterPosition, Vector3 damageReceiverPosition)
    {
        DamageIndicator spawnedDamagedIndicator = Instantiate(damageIndicatorPrefab, transform).GetComponent<DamageIndicator>();
        spawnedDamagedIndicator.Spawn(damage, damageInflicterPosition, damageReceiverPosition);
    }
}
