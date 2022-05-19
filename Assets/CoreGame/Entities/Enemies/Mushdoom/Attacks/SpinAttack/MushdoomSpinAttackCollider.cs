using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushdoomSpinAttackCollider : MonoBehaviour
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
        if (other.CompareTag(TagManager.PLAYER_HURTBOX))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage);
        }
        else if (other.CompareTag(TagManager.SHIELD))
        {
            Shield shield = other.GetComponent<Shield>();
            Vector3 hitPosition = other.ClosestPointOnBounds(transform.position);
            shield.HitShield(hitPosition);
        }
    }
}
