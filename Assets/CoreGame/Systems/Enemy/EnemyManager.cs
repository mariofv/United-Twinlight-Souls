using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnVFXPool spawnVFXPool;
    [SerializeField] private EnemyPool mushdoomPool;
    [SerializeField] private EnemyPool biterPool;
    [SerializeField] private EnemyPool necroplantPool;

    private List<Enemy> spawnedEnemies;

    void Awake()
    {
        spawnedEnemies = new List<Enemy>();
    }

    public void SpawnEnemy(EnemyAsset.EnemyId enemyId, bool spawnAnimation = true)
    {
        SpawnEnemy(enemyId, NavMeshHelper.FindAvailableSpawnPosition(), spawnAnimation);
    }

    public Enemy SpawnEnemy(EnemyAsset.EnemyId enemyId, Vector3 spawnPosition, bool spawnAnimation = true)
    {
        Enemy spawnedEnemy = SpawnEnemyFromPool(enemyId);

        spawnedEnemy.Spawn(spawnPosition, spawnAnimation);
        spawnedEnemies.Add(spawnedEnemy);

        EnemySpawnVFX spawnVFX = spawnVFXPool.GetVFXInstance();
        spawnVFX.transform.position = spawnPosition;
        spawnVFX.PlayEffect();

        return spawnedEnemy;
    }

    private Enemy SpawnEnemyFromPool(EnemyAsset.EnemyId enemyId)
    {
        switch (enemyId)
        {
            case EnemyAsset.EnemyId.MUSHDOOM:
                return mushdoomPool.GetEnemyInstance();

            case EnemyAsset.EnemyId.BITER:
                return biterPool.GetEnemyInstance();

            case EnemyAsset.EnemyId.NECROPLANT:
                return necroplantPool.GetEnemyInstance();

            default:
                throw new UnityException("Incorrect EnemyType!");
        }
    }

    public Enemy GetClosestEnemy(Vector3 postion)
    {
        Enemy closestEnemy = null;
        float closestEnemySqrDistance = float.PositiveInfinity;

        for (int i = 0; i < spawnedEnemies.Count; ++i)
        {
            float currentSqrDistance = Vector3.SqrMagnitude(spawnedEnemies[i].transform.position - postion);
            if (currentSqrDistance < closestEnemySqrDistance)
            {
                closestEnemySqrDistance = currentSqrDistance;
                closestEnemy = spawnedEnemies[i];
            }
        }

        return closestEnemy;
    }

    public void InitializedEnemyPools()
    {
        spawnVFXPool.InitPool();
        mushdoomPool.InitPool();
        biterPool.InitPool();
        necroplantPool.InitPool();
    }

    public void EmptyEnemyPools()
    {
        spawnVFXPool.EmptyPool();
        mushdoomPool.EmptyPool();
        biterPool.EmptyPool();
        necroplantPool.EmptyPool();
    }

    public void RemoveSpawnedEnemy(Enemy enemyToBeRemoved)
    {
        spawnedEnemies.Remove(enemyToBeRemoved);
    }

    public void KillAllEnemies(bool spawnLoot = true)
    {
        while (spawnedEnemies.Count != 0)
        {
            spawnedEnemies[0].Kill(spawnLoot);
        }

        spawnedEnemies.Clear();
    }
}
