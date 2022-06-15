using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthquakeShockwave : MonoBehaviour
{
    [SerializeField] private BossEarthquake earthquake;
    [SerializeField] private float attackRadius;
    [SerializeField] private float safeRadius;
    private bool shockwaveColliderActive = false;
    bool hasShockwaveHitPlayer = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.instance.player.GetControlledCharacter().transform;
    }

    public void Trigger()
    {
        hasShockwaveHitPlayer = false;
        shockwaveColliderActive = true;
    }

    public void End()
    {
        shockwaveColliderActive = false;
    }

    private void Update()
    {
        if (!shockwaveColliderActive || hasShockwaveHitPlayer)
        {
            return;
        }

        Vector3 playerPosition = playerTransform.position;
        if (playerPosition.y >= 1f)
        {
            return;
        }

        Vector3 shockwaveCenterToPlayer = playerTransform.position - transform.position;
        shockwaveCenterToPlayer.y = 0;
        float sqrDistance = Vector3.SqrMagnitude(shockwaveCenterToPlayer);

        float scaledSafeRadius = safeRadius * transform.localScale.x;
        float scaledAttackRadius = attackRadius * transform.localScale.x;

        if (sqrDistance >= (scaledSafeRadius * scaledSafeRadius) && sqrDistance <= (scaledAttackRadius * scaledAttackRadius))
        {
            earthquake.OnShockwaveHit();
            hasShockwaveHitPlayer = true;
        }
    }
}
