using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLeftPunch : MonoBehaviour
{
    private enum LeftPunchState
    {
        INNACTIVE,
        ACTIVE
    }

    [SerializeField] private BossAttackRockPool bossAttackRockPool;
    [SerializeField] private List<int> attackWaves;
    [SerializeField] private float timeBetweenWaves;
    private int currentAttackWave = 0;

    [SerializeField] private float attackLength;
    private float attackLengthStep;
    [SerializeField] private float attackArch;
    [SerializeField] private float minAttackAngle;
    [SerializeField] private float maxAttackAngle;
    private Vector3 attackDirection;

    private LeftPunchState attackState;
    private float currentTime = 0f;

    private void Awake()
    {
        attackLengthStep = attackLength / attackWaves.Count;
    }

    private void Update()
    {
        switch (attackState)
        {
            case LeftPunchState.INNACTIVE:
                break;
            case LeftPunchState.ACTIVE:
                currentTime += Time.deltaTime;
                if (currentTime >= timeBetweenWaves)
                {
                    SpawnAttackWave(currentAttackWave);
                    ++currentAttackWave;
                    currentTime -= timeBetweenWaves;

                    if (currentAttackWave == attackWaves.Count)
                    {
                        attackState = LeftPunchState.INNACTIVE;
                    }
                }
                break;
        }
    }

    public void Trigger()
    {
        attackState = LeftPunchState.ACTIVE;
        currentTime = 0f;
        currentAttackWave = 0;

        attackDirection = (GameManager.instance.player.GetControlledCharacter().transform.position - transform.position).normalized;

        float attackRandomAngle = Random.Range(minAttackAngle, maxAttackAngle);
        attackDirection = Quaternion.AngleAxis(attackRandomAngle, Vector3.up) * attackDirection;
    }

    private void SpawnAttackWave(int waveIndex)
    {
        float waveAttackLength = attackLengthStep * (waveIndex + 1);
        float currentWaveAttackArchStep = attackArch / attackWaves[waveIndex];

        for (int i = 0; i < attackWaves[waveIndex]; ++i)
        {
            float currentAttackRockArch = currentWaveAttackArchStep * i - attackArch * 0.5f;
            Quaternion currentAttackRockRotation = Quaternion.AngleAxis(currentAttackRockArch, Vector3.up);
            Vector3 currentAttackPosition = transform.position + currentAttackRockRotation * attackDirection * waveAttackLength;
            currentAttackPosition.y += 0.125f;

            BossAttackRock bossAttackRock = bossAttackRockPool.GetInstance();
            bossAttackRock.Spawn(currentAttackPosition, true);
        }

    }
}
