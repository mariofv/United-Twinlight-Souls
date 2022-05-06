using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyAsset.EnemyId enemyId;

    public Enemy Spawn(bool spawnAnimation = true)
    {
        return GameManager.instance.enemyManager.SpawnEnemy(enemyId, transform.position, spawnAnimation);
    }
}
