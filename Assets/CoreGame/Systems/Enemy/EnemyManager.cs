using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyPool mushdoomPool;
    [SerializeField] private EnemyPool bitterPool;
    [SerializeField] private EnemyPool necroplantPool;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(EnemyAsset.EnemyId enemyId, bool spawnAnimation = true)
    {
        SpawnEnemy(enemyId, NavMeshHelper.FindAvailableSpawnPosition(), spawnAnimation);
    }

    public Enemy SpawnEnemy(EnemyAsset.EnemyId enemyId, Vector3 spawnPosition, bool spawnAnimation = true)
    {
        Enemy spawnedEnemy = SpawnEnemyFromPool(enemyId);

        spawnedEnemy.Spawn(spawnPosition, spawnAnimation);

        return spawnedEnemy;
    }

    private Enemy SpawnEnemyFromPool(EnemyAsset.EnemyId enemyId)
    {
        switch (enemyId)
        {
            case EnemyAsset.EnemyId.MUSHDOOM:
                return mushdoomPool.GetEnemyInstance();

            case EnemyAsset.EnemyId.BITTER:
                return bitterPool.GetEnemyInstance();

            case EnemyAsset.EnemyId.NECROPLANT:
                return necroplantPool.GetEnemyInstance();

            default:
                throw new UnityException("Incorrect EnemyType!");
        }
    }

    public void InitializedEnemyPools()
    {
        mushdoomPool.InitPool();
        bitterPool.InitPool();
        necroplantPool.InitPool();
    }

    public void EmptyEnemyPools()
    {
        mushdoomPool.EmptyPool();
        bitterPool.EmptyPool();
        necroplantPool.EmptyPool();
    }
}
