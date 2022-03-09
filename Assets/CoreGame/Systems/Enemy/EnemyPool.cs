using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyAsset poolEnemyAsset;
    [SerializeField] private int batchSize;

    private List<Enemy> instantiatedEnemies;
    private bool isInitialized = false;

    public void InitPool()
    {
        if (isInitialized)
        {
            return;
        }

        instantiatedEnemies = new List<Enemy>();
        AllocateNewBatch();

        isInitialized = true;
    }

    public void EmptyPool()
    {
        if (!isInitialized)
        {
            return;
        }

        for (int i = 0; i < instantiatedEnemies.Count; ++i)
        {
            Destroy(instantiatedEnemies[i].gameObject);
        }

        instantiatedEnemies = null;

        isInitialized = false;
    }

    public Enemy GetEnemyInstance()
    {
        int instantiatedEnemiesOriginalSize = instantiatedEnemies.Count;

        for (int i = 0; i < instantiatedEnemiesOriginalSize; ++i)
        {
            if (!instantiatedEnemies[i].IsAlive())
            {
                return instantiatedEnemies[i];
            }
        }

        AllocateNewBatch();
        return instantiatedEnemies[instantiatedEnemiesOriginalSize];
    }

    private void AllocateNewBatch()
    {
        for (int i = 0; i < batchSize; ++i)
        {
            GameObject newEnemyInstanceGameObject = GameObject.Instantiate(poolEnemyAsset.enemyPrefab, transform);
            newEnemyInstanceGameObject.SetActive(false);

            Enemy newEnemyInstance = newEnemyInstanceGameObject.GetComponent<Enemy>();

            instantiatedEnemies.Add(newEnemyInstance);
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}
