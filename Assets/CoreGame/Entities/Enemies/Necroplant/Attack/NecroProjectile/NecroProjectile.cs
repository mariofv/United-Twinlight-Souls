using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroProjectile : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootHit1;
    [SerializeField] private AudioClip shootHit2;
    [SerializeField] private AudioClip shootHit3;
    [SerializeField] private AudioClip shootHit4;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;
    [SerializeField] private List<ParticleSystem> projectileEffects;

    [Header("Impact")]
    [SerializeField] private float destroyingTime;
    [SerializeField] private List<ParticleSystem> impactEffects;
    private float currentTime = 0f;

    private Vector3 projectileDirection;
    private bool alive = false;
    private bool destroyed = false;

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            transform.position += Time.deltaTime * projectileSpeed * projectileDirection;
        }

        else if (destroyed)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= destroyingTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Shoot(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        projectileDirection = direction;
        alive = true;
    }

    private void DestroyProjectile()
    {
        audioSource.PlayOneShotRandom(shootHit1, shootHit2, shootHit3, shootHit4);
        for (int i = 0; i < projectileEffects.Count; ++i)
        {
            projectileEffects[i].Stop();
            projectileEffects[i].Clear();
        }

        for (int i = 0; i < impactEffects.Count; ++i)
        {
            impactEffects[i].Play();
        }

        alive = false;
        destroyed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(TagManager.PLAYER_HURTBOX))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(projectileDamage);
            DestroyProjectile();
        }
        else if (collision.transform.CompareTag(TagManager.LEVEL_COLLIDER))
        {
            DestroyProjectile();
        }
    }
}
