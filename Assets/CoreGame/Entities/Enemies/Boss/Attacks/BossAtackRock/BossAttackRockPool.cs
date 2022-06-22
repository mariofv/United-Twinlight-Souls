using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRockPool : MonoBehaviour
{
    [SerializeField] private GameObject bossAttackRockPrefab;
    [SerializeField] private int batchSize;

    private List<BossAttackRock> instantiatedAttackRocks;
    private bool isInitialized = false;
    public void InitPool()
    {
        if (isInitialized)
        {
            return;
        }

        instantiatedAttackRocks = new List<BossAttackRock>();
        AllocateNewBatch();

        isInitialized = true;
    }

    public void EmptyPool()
    {
        if (!isInitialized)
        {
            return;
        }

        for (int i = 0; i < instantiatedAttackRocks.Count; ++i)
        {
            Destroy(instantiatedAttackRocks[i].gameObject);
        }

        instantiatedAttackRocks = null;

        isInitialized = false;
    }

    public BossAttackRock GetInstance()
    {
        int instantiatedAttackRocksOriginalSize = instantiatedAttackRocks.Count;

        for (int i = 0; i < instantiatedAttackRocksOriginalSize; ++i)
        {
            if (!instantiatedAttackRocks[i].IsActive())
            {
                return instantiatedAttackRocks[i];
            }
        }

        AllocateNewBatch();
        return instantiatedAttackRocks[instantiatedAttackRocksOriginalSize];
    }

    private void AllocateNewBatch()
    {
        for (int i = 0; i < batchSize; ++i)
        {
            BossAttackRock newBossAttackRockInstance = Instantiate(bossAttackRockPrefab, transform).GetComponent<BossAttackRock>();
            newBossAttackRockInstance.transform.position = Vector3.up * 100000f;
            instantiatedAttackRocks.Add(newBossAttackRockInstance);
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}
