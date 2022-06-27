using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : MonoBehaviour
{
    [Range(0, 100)] public int damage;

    [Range(0f, 1f)] public float minimunProgressToChain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.ENEMY_HURTBOX))
        {
            EnemyHurtbox collidedEnemyHurtBox = other.GetComponent<EnemyHurtbox>();
            collidedEnemyHurtBox.GetEnemyScript().Hurt(damage);
            GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.MILD, 0.5f);
        }
    }
}
