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

    public void SpawnEnemy(EnemyAsset.EnemyId enemyId)
    {
        SpawnEnemy(enemyId, FindAvailableSpawnPosition());
    }

    public void SpawnEnemy(EnemyAsset.EnemyId enemyId, Vector3 spawnPosition)
    {
        Enemy spawnedEnemy = SpawnEnemyFromPool(enemyId);

        spawnedEnemy.Teleport(spawnPosition);
        spawnedEnemy.Reanimate();
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

    private Vector3 FindAvailableSpawnPosition()
    {
        Vector3 playerPosition = GameManager.instance.player.Character().characterMovementManager.GetPosition();

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(playerPosition, out navMeshHit, 5f, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            throw new UnityException("Cannot spawn enemy because there is no suitable point near player!");
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
