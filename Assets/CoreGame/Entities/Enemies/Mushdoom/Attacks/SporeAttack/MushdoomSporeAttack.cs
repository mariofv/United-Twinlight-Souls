using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MushdoomSporeAttack : MonoBehaviour
{
    [SerializeField] private SphereCollider sporeAttackVolume;
    [SerializeField] private ParticleSystem sporesParticleSystem;
    [SerializeField] private ParticleSystem cloudParticleSystem;

    public UnityEvent onAttackEnd;

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
        //TODO: Don't forget to delete this from that scene before changing level in order to avoid deleting them
        transform.parent = GameManager.instance.levelManager.GetCurrentLevel().spawnedEntitiesHolder;
        transform.position = position;
        currentTime = 0f;
        sporeAttackVolume.enabled = true;
        sporesParticleSystem.Play();
        cloudParticleSystem.Play();
    }

    public void Despawn()
    {
        sporeAttackVolume.enabled = false;
        sporesParticleSystem.Stop();
        cloudParticleSystem.Stop();
        onAttackEnd.Invoke();
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
            currentTickTime += Time.fixedDeltaTime;
            if (currentTickTime >= damageTick)
            {
                currentTickTime = 0f;
                GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage);
            }
        }
    }
}
