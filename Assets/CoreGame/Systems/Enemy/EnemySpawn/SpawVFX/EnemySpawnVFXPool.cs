using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnVFXPool : MonoBehaviour
{
    [SerializeField] private GameObject poolVFXAsset;
    [SerializeField] private int batchSize;

    private List<EnemySpawnVFX> instantiatedVFXs;
    private bool isInitialized = false;

    public void InitPool()
    {
        if (isInitialized)
        {
            return;
        }

        instantiatedVFXs = new List<EnemySpawnVFX>();
        AllocateNewBatch();

        isInitialized = true;
    }

    public void EmptyPool()
    {
        if (!isInitialized)
        {
            return;
        }

        for (int i = 0; i < instantiatedVFXs.Count; ++i)
        {
            Destroy(instantiatedVFXs[i].gameObject);
        }

        instantiatedVFXs = null;

        isInitialized = false;
    }

    public EnemySpawnVFX GetVFXInstance()
    {
        int instantiatedVFXsOriginalSize = instantiatedVFXs.Count;

        for (int i = 0; i < instantiatedVFXsOriginalSize; ++i)
        {
            if (instantiatedVFXs[i].IsAvailable())
            {
                return instantiatedVFXs[i];
            }
        }

        AllocateNewBatch();
        return instantiatedVFXs[instantiatedVFXsOriginalSize];
    }

    private void AllocateNewBatch()
    {
        for (int i = 0; i < batchSize; ++i)
        {
            EnemySpawnVFX newVFXInstance = Instantiate(poolVFXAsset, transform).GetComponent<EnemySpawnVFX>();
            instantiatedVFXs.Add(newVFXInstance);
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}
