using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthquake : MonoBehaviour
{
    private enum BossEarthquakeState
    {
        INACTIVE,
        ACTIVE
    }

    [SerializeField] private int attackDamage;
    [SerializeField] private float hitBoxDuration;
    [SerializeField] private BoxCollider attackCollider;


    [SerializeField] private float attackTime;
    [SerializeField] private float shockwaveSpeed;

    [SerializeField] private MeshRenderer crackMeshRenderer;
    [SerializeField] private MeshRenderer shockwaveMeshRenderer;

    private Material crackMaterial;
    private Transform shockwaveTransform;
    private Material shockwaveMaterial;

    private float currentTime = 0f;
    private BossEarthquakeState currentState;
    private bool hasAttackHurtPlayer = false;

    private void Awake()
    {
        crackMaterial = crackMeshRenderer.material;
        crackMaterial.SetAlpha(0f);

        shockwaveTransform = shockwaveMeshRenderer.transform;
        shockwaveMaterial = shockwaveMeshRenderer.material;
        shockwaveMaterial.SetAlpha(0f);

        attackCollider.enabled = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossEarthquakeState.INACTIVE:
                break;
            case BossEarthquakeState.ACTIVE:
                
                currentTime += Time.deltaTime;
                float progress = Mathf.Min(1f, currentTime / attackTime);

                if (currentTime >= hitBoxDuration)
                {
                    attackCollider.enabled = false;
                }

                crackMaterial.SetAlpha(1f - progress);

                shockwaveMaterial.SetAlpha(1f - progress);
                float shockwaveScale = 1f + progress * shockwaveSpeed;
                shockwaveTransform.localScale = new Vector3(shockwaveScale, 1f, shockwaveScale);

                if (progress == 1f)
                {
                    currentState = BossEarthquakeState.INACTIVE;
                }
                break;
        }
    }

    public void Trigger()
    {
        currentState = BossEarthquakeState.ACTIVE;
        hasAttackHurtPlayer = false;
        currentTime = 0f;
        attackCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAttackHurtPlayer && other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(attackDamage, transform.position, attackCausesStun:true);
            hasAttackHurtPlayer = true;
        }
    }
}
