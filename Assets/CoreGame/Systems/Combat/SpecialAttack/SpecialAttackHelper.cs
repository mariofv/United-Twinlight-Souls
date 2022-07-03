using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackHelper : MonoBehaviour
{
    [SerializeField] private SpecialAttack specialAttack;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(TagManager.LEVEL_COLLIDER))
        {
            specialAttack.Stop();
        }
    }
}
