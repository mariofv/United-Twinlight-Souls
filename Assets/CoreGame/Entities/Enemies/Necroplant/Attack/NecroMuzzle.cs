using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMuzzle : MonoBehaviour
{
    [SerializeField] private Transform necroMuzzleCannon;
    [SerializeField] private List<ParticleSystem> necroMuzzleEffects;
    [SerializeField] private GameObject necroProjectilePrefab;

    public void Shoot(Vector3 direction)
    {
        for (int i = 0; i < necroMuzzleEffects.Count; ++i)
        {
            necroMuzzleEffects[i].Play();
        }

        GameObject instantiatedProjectile = Instantiate(necroProjectilePrefab);
        instantiatedProjectile.transform.position = necroMuzzleCannon.position;
        NecroProjectile instantiatedNecroProjectile = instantiatedProjectile.GetComponent<NecroProjectile>();
        instantiatedNecroProjectile.Shoot(direction);
    }
}
