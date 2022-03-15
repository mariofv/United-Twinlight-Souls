using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushdoomSporeAttack : MonoBehaviour
{
    [SerializeField] private SphereCollider sporeAttackVolume;

    private float currentTime = 0f;
    private float duration;

    private int damage;
    private float damageTick;
    private float currentTickTime = 0f;

    private void Update()
    {
        if (sporeAttackVolume.enabled)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= duration)
            {
                Despawn();
            }
        }
    }

    public void SetParameters(float duration, float radius, int damage, float damageTick)
    {
        this.duration = duration;
        sporeAttackVolume.radius = radius;
    
        this.damage = damage;
        this.damageTick = damageTick;
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        currentTime = 0f;
        sporeAttackVolume.enabled = true;
    }

    public void Despawn()
    {
        sporeAttackVolume.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            currentTickTime = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            currentTime += Time.fixedDeltaTime;
            if (currentTime >= damageTick)
            {
                currentTickTime = 0f;
                GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage);
            }
        }
    }
}
