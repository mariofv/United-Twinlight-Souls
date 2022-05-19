using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(damage, other.transform.position);
            if (!GameManager.instance.player.GetControlledCharacter().IsDead())
            {
                GameManager.instance.levelManager.Respawn();
            }
        }
    }
}
