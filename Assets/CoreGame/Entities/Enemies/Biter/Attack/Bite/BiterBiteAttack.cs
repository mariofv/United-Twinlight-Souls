using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiterBiteAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider attackHitbox;
    private int damage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetColliderActive(bool active)
    {
        attackHitbox.enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_HURTBOX) || other.CompareTag(TagManager.SHIELD))
        {
            Vector3 hitPosition = other.ClosestPointOnBounds(transform.position);
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage, hitPosition);
        }
    }
}
