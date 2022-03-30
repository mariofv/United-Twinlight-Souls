using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyAsset.EnemyId enemyId;
    [SerializeField] private bool spawnFromStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnFromStart)
        {
            Spawn();
        }
    }

    public Enemy Spawn()
    {
        return GameManager.instance.enemyManager.SpawnEnemy(enemyId, transform.position);
    }
}
