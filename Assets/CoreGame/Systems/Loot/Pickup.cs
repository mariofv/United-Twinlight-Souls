using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private SphereCollider pickupCollider;

    private void Pick()
    {
        ApplyEffect();
        pickupCollider.enabled = false;
    }

    protected abstract void ApplyEffect();

    public virtual void Spawn(Vector3 position)
    {
        transform.position = position;
        pickupCollider.enabled = true;
    }

    public bool IsActive()
    {
        return pickupCollider.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            Pick();
        }
    }
}
