using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiterExplosionAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<ParticleSystem> particleSystems;
    [SerializeField] private Collider explosionCollider;
    public UnityEvent onExplosionEndEvent;

    [Header("Parameters")]
    [SerializeField] private float explosionTime;
    [SerializeField] private float endExplosionTime;
    private int damage;

    private bool exploding = false;
    private bool alreadyExploded = false;
    private float currentTime = 0f;

    private void Update()
    {
        if (exploding)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= explosionTime && !alreadyExploded)
            {
                OnExplosion();
            }
            if (currentTime >= endExplosionTime)
            {
                OnExplosionEnd();
            }
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void OnExplosionStart()
    {
        for (int i = 0; i < particleSystems.Count; ++i)
        {
            particleSystems[i].Clear();
            particleSystems[i].Play();
        }

        currentTime = 0f;
        exploding = true;
        alreadyExploded = false;
    }

    private void OnExplosion()
    {
        explosionCollider.enabled = true;
        alreadyExploded = false;
    }

    private void OnExplosionEnd()
    {
        for (int i = 0; i < particleSystems.Count; ++i)
        {
         //   particleSystems[i].Stop();
        }
        explosionCollider.enabled = false;
        exploding = false;
        onExplosionEndEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage);
        }
    }
}
