using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPool : MonoBehaviour
{
    [SerializeField] private GameObject poolPrefab;
    [SerializeField] private int batchSize;

    private List<Pickup> instantiatedPickups;
    private bool isInitialized = false;

    public void InitPool()
    {
        if (isInitialized)
        {
            return;
        }

        instantiatedPickups = new List<Pickup>();
        AllocateNewBatch();

        isInitialized = true;
    }

    public void EmptyPool()
    {
        if (!isInitialized)
        {
            return;
        }

        for (int i = 0; i < instantiatedPickups.Count; ++i)
        {
            Destroy(instantiatedPickups[i].gameObject);
        }

        instantiatedPickups = null;

        isInitialized = false;
    }

    public Pickup GetPickupInstance()
    {
        int instantiatedPickupsOriginalSize = instantiatedPickups.Count;

        for (int i = 0; i < instantiatedPickupsOriginalSize; ++i)
        {
            if (!instantiatedPickups[i].IsActive())
            {
                return instantiatedPickups[i];
            }
        }

        AllocateNewBatch();
        return instantiatedPickups[instantiatedPickupsOriginalSize];
    }

    private void AllocateNewBatch()
    {
        for (int i = 0; i < batchSize; ++i)
        {
            Pickup newPickupInstance = Instantiate(poolPrefab, transform).GetComponent<Pickup>();

            newPickupInstance.transform.position = new Vector3(20000f, 0f, 0f);
            instantiatedPickups.Add(newPickupInstance);
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}
