using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private Transform thrownAttacksParent;
    [SerializeField] private List<ParticleSystem> specialAttackEffects;

    [SerializeField] private int damage;
    [SerializeField] private SphereCollider hitbox;

    [SerializeField] private float speed;
    [SerializeField] private float helpSpeed;
    private Transform target;
    private Vector3 direction;

    private bool triggered = false;

    private void Update()
    {
        if (direction == Vector3.zero)
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

    public void Cast(Transform holder)
    {
        if (triggered)
        {
            throw new UnityException("Special attack triggered when already triggered!");
        }

        triggered = true;
        transform.parent = holder;
        transform.localPosition = Vector3.zero;

        for (int i = 0; i < specialAttackEffects.Count; ++i)
        {
            specialAttackEffects[i].Play();
        }
    }

    public void Throw(Vector3 throwDirection, EnemyHurtbox targetHurtbox)
    {
        if (!triggered)
        {
            throw new UnityException("Special attack casted when not triggered!");
        }

        transform.parent = thrownAttacksParent;

        direction = throwDirection;
        target = targetHurtbox.transform;
        hitbox.enabled = true;
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
        hitbox.enabled = false;

        for (int i = 0; i < specialAttackEffects.Count; ++i)
        {
            specialAttackEffects[i].Stop();
        }
    }

    public bool IsAvailable()
    {
        return !triggered;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(TagManager.ENEMY_HURTBOX))
        {
            collision.transform.GetComponent<EnemyHurtbox>().GetEnemyScript().Hurt(damage);
            Stop();
        }
        else if (collision.transform.CompareTag(TagManager.LEVEL_COLLIDER))
        {
            Stop();
        }
    }
}
