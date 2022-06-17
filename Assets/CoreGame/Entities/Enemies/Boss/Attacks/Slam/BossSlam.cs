using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlam : MonoBehaviour
{
    private enum BossSlamState
    {
        INACTIVE,
        ACTIVE
    }

    [SerializeField] private float attackTime;

    [Header("Attack")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float hitBoxDuration;
    [SerializeField] private SphereCollider attackCollider;
    [SerializeField] private MeshRenderer attackMeshRenderer;

    private Material attackMaterial;

    private float currentTime = 0f;
    private BossSlamState currentState;
    private bool hasAttackHurtPlayer = false;

    private void Awake()
    {
        attackMaterial = attackMeshRenderer.material;
        attackMaterial.SetFloat("_FadeProgress", 1f);

        attackCollider.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossSlamState.INACTIVE:
                break;
            case BossSlamState.ACTIVE:

                currentTime += Time.deltaTime;
                float progress = Mathf.Min(1f, currentTime / attackTime);

                if (currentTime >= hitBoxDuration)
                {
                    attackCollider.enabled = false;
                }

                attackMaterial.SetFloat("_FadeProgress", progress);

                if (progress == 1f)
                {
                    currentState = BossSlamState.INACTIVE;
                }
                break;
        }
    }

    public void Trigger(Vector3 position)
    {
        transform.position = position;
        currentState = BossSlamState.ACTIVE;
        hasAttackHurtPlayer = false;
        currentTime = 0f;
        attackCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAttackHurtPlayer && other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(attackDamage, transform.position, attackCausesStun: false);
            hasAttackHurtPlayer = true;
        }
    }
}
