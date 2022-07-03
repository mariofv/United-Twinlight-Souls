using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private Transform thrownAttacksParent;
    [SerializeField] private List<ParticleSystem> specialAttackEffects;

    [SerializeField] private int damage;
    [SerializeField] private SphereCollider hitbox;
    [SerializeField] private SphereCollider projectileHitbox;
    [SerializeField] private float projectileLife;

    [SerializeField] private float speed;
    [SerializeField] private float helpSpeed;
    private Transform target;
    private Vector3 direction;

    private bool triggered = false;
    private float currentLifeTime = 0f;

    private void Update()
    {
        if (direction == Vector3.zero)
        {
            return;
        }

        currentLifeTime += Time.deltaTime;
        Vector3 directionToTarget = Vector3.zero;
        if (target != null)
        {
            directionToTarget = (target.position - transform.position).normalized;
        }

        transform.position += (speed * direction + helpSpeed * directionToTarget) * Time.deltaTime;

        if (currentLifeTime >= projectileLife)
        {
            Stop();
        }
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
        projectileHitbox.enabled = true; 
        currentLifeTime = 0f;
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
        projectileHitbox.enabled = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(TagManager.ENEMY_HURTBOX))
        {
            other.transform.GetComponent<EnemyHurtbox>().GetEnemyScript().Hurt(damage);
            Stop();
        }
    }
}
