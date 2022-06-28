using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> specialAttackVFXs;

    public void Trigger()
    {
        for (int i = 0; i < specialAttackVFXs.Count; ++i)
        {
            specialAttackVFXs[i].Play();
        }
    }

    public void Stop()
    {
        for (int i = 0; i < specialAttackVFXs.Count; ++i)
        {
            specialAttackVFXs[i].Stop();
        }
    }
}
