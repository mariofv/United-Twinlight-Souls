using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackHitbox : MonoBehaviour
{
    [SerializeField] private CharacterCombatManager characterCombatManager;
    [SerializeField] private int attackIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.ENEMY_HURTBOX))
        {
            EnemyHurtbox collidedEnemyHurtBox = other.GetComponent<EnemyHurtbox>();
            collidedEnemyHurtBox.GetEnemyScript().Hurt(characterCombatManager.GetLightAttackDamage(attackIndex));
            GameManager.instance.cameraManager.ShakeCamera(CameraManager.CameraShakeType.MILD, 0.5f);
        }
    }
}
