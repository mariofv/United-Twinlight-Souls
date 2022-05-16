using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private PickupPool lightOrbsPool;

    public void SpawnLightOrbs(Vector3 position, int lightOrbsAmount)
    {
        for (int i = 0; i < lightOrbsAmount; ++i)
        {
            LightOrb spawnedLightOrb = (LightOrb)lightOrbsPool.GetPickupInstance();
            spawnedLightOrb.Spawn(position);
        }
    }

    public void InitializePickupPools()
    {
        lightOrbsPool.InitPool();
    }

    public void EmptyPickupPools()
    {
        lightOrbsPool.EmptyPool();
    }
}
