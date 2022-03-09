using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAsset", menuName = "EnemyAsset", order = 1)]
[System.Serializable]
public class EnemyAsset : ScriptableObject
{
    public enum EnemyId
    {
        MUSHDOOM,
        BITTER,
        NECROPLANT
    }

    public EnemyId enemyId;
    public GameObject enemyPrefab;
}
