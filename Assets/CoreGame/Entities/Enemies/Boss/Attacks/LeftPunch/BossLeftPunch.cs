using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLeftPunch : MonoBehaviour
{
    [SerializeField] private BossAttackRockPool bossAttackRockPool;

    private void Awake()
    {
        bossAttackRockPool.InitPool();
    }

    public void Trigger(Vector3 position)
    {
        BossAttackRock bossAttackRock = bossAttackRockPool.GetInstance();
        bossAttackRock.Spawn(position, false);
    }
}
