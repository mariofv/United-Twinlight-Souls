using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAvalanche : MonoBehaviour
{
    private enum AvalancheState
    {
        INNACTIVE,
        ACTIVE
    }

    [Header("Rocks")]
    [SerializeField] private BossAttackRockPool bossAttackRockPool;
    [SerializeField] private float rockSpawningAcceleration;
    [SerializeField] private float attackRockOffset;
    [SerializeField] private float timeBetweenRocks;
    [SerializeField] private float minTimeBetweenRocks;

    [Header("Measures")]
    [SerializeField] private float minAttackRadius;
    [SerializeField] private float maxAttackRadius;
    [SerializeField] private float attackArch;
    private float currentTimeBetweenRocks;


    private AvalancheState attackState;
    private float currentTime = 0f;

    private void Update()
    {
        switch (attackState)
        {
            case AvalancheState.INNACTIVE:
                break;
            case AvalancheState.ACTIVE:
                currentTime += Time.deltaTime;
                currentTimeBetweenRocks = Mathf.Max(minTimeBetweenRocks, currentTimeBetweenRocks - rockSpawningAcceleration * Time.deltaTime);
                if (currentTime >= currentTimeBetweenRocks)
                {
                    SpawnAttackRock();
                    currentTime -= currentTimeBetweenRocks;
                }
                break;
        }
    }

    public void Trigger()
    {
        attackState = AvalancheState.ACTIVE;
        currentTime = 0f;
        currentTimeBetweenRocks = timeBetweenRocks;
    }


    public void End()
    {
        attackState = AvalancheState.INNACTIVE;
    }

    private void SpawnAttackRock()
    {
        float randomArch = Random.Range(0f, attackArch) - attackArch * 0.5f;
        float randomRadius = Random.Range(minAttackRadius, maxAttackRadius);
        Vector3 attackOffset = Quaternion.AngleAxis(randomArch, Vector3.up) * Vector3.forward * randomRadius;

        Vector3 rockPosition = transform.position + attackOffset;
        rockPosition.y += attackRockOffset;
        BossAttackRock bossAttackRock = bossAttackRockPool.GetInstance();
        bossAttackRock.Spawn(rockPosition, false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 centerLineOrigin = transform.position + Vector3.forward * minAttackRadius;
        Vector3 centerLineEnd = transform.position + Vector3.forward * maxAttackRadius;
        Gizmos.DrawLine(centerLineOrigin, centerLineEnd);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Quaternion.AngleAxis(-attackArch * 0.5f, Vector3.up) * centerLineOrigin, Quaternion.AngleAxis(-attackArch * 0.5f, Vector3.up) * centerLineEnd);
        Gizmos.DrawLine(Quaternion.AngleAxis(attackArch * 0.5f, Vector3.up) * centerLineOrigin, Quaternion.AngleAxis(attackArch * 0.5f, Vector3.up) * centerLineEnd);
        
    }
}
