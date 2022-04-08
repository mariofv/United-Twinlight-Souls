using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;
    private Vector3 projectileDirection;
    private bool alive = false;

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            transform.position += Time.deltaTime * projectileSpeed * projectileDirection;
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
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(TagManager.PLAYER))
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
