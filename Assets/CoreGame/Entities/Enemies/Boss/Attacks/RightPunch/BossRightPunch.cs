using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRightPunch : MonoBehaviour
{
    private enum RightPunchState
    {
        INNACTIVE,
        ACTIVE
    }

    [SerializeField] private BossAttackRockPool bossAttackRockPool;
    [SerializeField] private int numberOfRocks;
    [SerializeField] private float timeBetweenRocks;
    private int currentAttackRock = 0;


    private RightPunchState attackState;
    private float currentTime = 0f;

    private void Awake()
    {
        bossAttackRockPool.InitPool();
    }

    private void Update()
    {
        switch (attackState)
        {
            case RightPunchState.INNACTIVE:
                break;
            case RightPunchState.ACTIVE:
                currentTime += Time.deltaTime;
                if (currentTime >= timeBetweenRocks)
                {
                    SpawnAttackRock();
                    ++currentAttackRock;
                    currentTime -= timeBetweenRocks;

                    if (currentAttackRock == numberOfRocks)
                    {
                        attackState = RightPunchState.INNACTIVE;
                    }
                }
                break;
        }
    }

    public void Trigger()
    {
        attackState = RightPunchState.ACTIVE;
        currentTime = 0f;
        currentAttackRock = 0;
    }

    private void SpawnAttackRock()
    {
        Vector3 playerPosition = GameManager.instance.player.GetControlledCharacter().transform.position;
        BossAttackRock bossAttackRock = bossAttackRockPool.GetInstance();
        bossAttackRock.Spawn(playerPosition, true);
    }
}
