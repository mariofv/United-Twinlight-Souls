using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyAsset.EnemyId enemyId;
    [SerializeField] private bool spawnFromStart = false;

    public Enemy Spawn(bool spawnAnimation = true)
    {
        return GameManager.instance.enemyManager.SpawnEnemy(enemyId, transform.position, spawnAnimation);
    }
}
