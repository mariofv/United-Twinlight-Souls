using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> specialAttackVFXs;
    
    [SerializeField] private float speed;
    [SerializeField] private float helpSpeed;
    private Transform target;
    private Vector3 direction;

    private bool triggered = false;

    private void Update()
    {
        if (!triggered)
        {
            return;
        }

        Vector3 directionToTarget = Vector3.zero;
        if (target != null)
        {
            directionToTarget = (target.position - transform.position).normalized;
        }

        transform.position += (speed * direction + helpSpeed * directionToTarget) * Time.deltaTime;
    }

    public void Trigger(Vector3 throwDirection, EnemyHurtbox targetHurtbox)
    {
        if (triggered)
        {
            throw new UnityException("Special attack triggered when already triggered!");
        }

        triggered = true;
        direction = throwDirection;
        target = targetHurtbox.transform;

        for (int i = 0; i < specialAttackVFXs.Count; ++i)
        {
            specialAttackVFXs[i].Play();
        }
    }

    public void Stop()
    {
        if (!triggered)
        {
            throw new UnityException("Special attack stopped when not triggered!");
        }

        triggered = false;
        target = null;
        direction = Vector3.zero;

        for (int i = 0; i < specialAttackVFXs.Count; ++i)
        {
            specialAttackVFXs[i].Stop();
        }
    }
}
