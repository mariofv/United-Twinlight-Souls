using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMuzzle : MonoBehaviour
{
    [SerializeField] private Transform necroMuzzleCannon;
    [SerializeField] private GameObject necroProjectilePrefab;

    public void Shoot(Vector3 direction)
    {
        GameObject instantiatedProjectile = Instantiate(necroProjectilePrefab);
        NecroProjectile instantiatedNecroProjectile = instantiatedProjectile.GetComponent<NecroProjectile>();
        instantiatedNecroProjectile.Shoot(direction);
    }
}
