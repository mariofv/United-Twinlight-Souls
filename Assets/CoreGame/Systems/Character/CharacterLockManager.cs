using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLockManager : CharacterSubManager
{
    private enum LockState
    {
        NONE,
        SOFT_LOCK,
        LOCK
    }

    [SerializeField] private float minSoftLockDistance;
    [SerializeField] [Range(0f, 180f)] private float maxAngleFrontOfPlayer;
    [SerializeField] [Range(0f, 1f)] private float frontOfPlayerAddedPriority;
    [SerializeField] private float timeUntilSoftLockCheck;

    private LockState currentState;

    private float currentSoftLockCheckTime = 0f;
    private Enemy currentSoftLockedEnemy = null;

    private Enemy currentLockedEnemy = null;

    // Update is called once per frame
    void Update()
    {
        if (currentState != LockState.LOCK)
        {
            currentSoftLockCheckTime += Time.deltaTime;
            if (currentSoftLockCheckTime >= timeUntilSoftLockCheck)
            {
                currentSoftLockCheckTime = 0f;
                CheckPotentialLockedEnemy();
            }
        }
    }

    public void SwitchLockEnemy()
    {
        if (currentState != LockState.LOCK)
        {
            Enemy softLockedEnemy = currentSoftLockedEnemy;
            if (softLockedEnemy == null)
            {
                return;
            }

            SoftUnlockEnemy();
            LockEnemy(softLockedEnemy);
        }
        else
        {
            UnlockEnemy();
        }
    }

    public void ChangeLockedEnemy()
    {
        if (currentState != LockState.LOCK)
        {
            return;
        }

        Enemy nextEnemy = GetNextEnemy(currentLockedEnemy);
        if (nextEnemy != null)
        {
            UnlockEnemy();
            LockEnemy(nextEnemy);
        }
    }

    private void SoftLockEnemy(Enemy enemy)
    {
        currentSoftLockedEnemy = enemy;
        currentSoftLockedEnemy.onSpawnedEnemyDead.AddListener(OnSoftLockedEnemyDeath);

        GameManager.instance.uiManager.gameUIManager.lockUI.SoftLockEnemy(currentSoftLockedEnemy.transform);
        currentState = LockState.SOFT_LOCK;
    }

    private void SoftUnlockEnemy()
    {
        currentSoftLockedEnemy.onSpawnedEnemyDead.RemoveListener(OnSoftLockedEnemyDeath);
        currentSoftLockedEnemy = null;

        GameManager.instance.uiManager.gameUIManager.lockUI.UnlockEnemy();
        currentState = LockState.NONE;
    }

    private void OnSoftLockedEnemyDeath()
    {
        SoftUnlockEnemy();
    }

    private void LockEnemy(Enemy enemy)
    {
        currentLockedEnemy = enemy;
        currentLockedEnemy.onSpawnedEnemyDead.AddListener(OnLockedEnemyDeath);

        GameManager.instance.uiManager.gameUIManager.lockUI.LockEnemy(currentLockedEnemy.transform);
        currentState = LockState.LOCK;
    }

    private void UnlockEnemy()
    {
        currentLockedEnemy.onSpawnedEnemyDead.RemoveListener(OnLockedEnemyDeath);
        currentLockedEnemy = null;

        GameManager.instance.uiManager.gameUIManager.lockUI.UnlockEnemy();
        currentState = LockState.NONE;
    }

    private void OnLockedEnemyDeath()
    {
        UnlockEnemy();
        Enemy bestSuitableEnemy = GetBestSuitableLockEnemy();
        if (bestSuitableEnemy != null)
        {
            LockEnemy(bestSuitableEnemy);
        }
    }

    private void CheckPotentialLockedEnemy()
    {
        Enemy bestSuitableEnemy = GetBestSuitableLockEnemy();
        if (bestSuitableEnemy != null)
        {
            if (currentState == LockState.SOFT_LOCK)
            {
                if (bestSuitableEnemy != currentSoftLockedEnemy)
                {
                    SoftUnlockEnemy();
                    SoftLockEnemy(bestSuitableEnemy);
                }
            }
            else
            {
                SoftLockEnemy(bestSuitableEnemy);
            }
        }
        else
        {
            if (currentState == LockState.SOFT_LOCK)
            {
                SoftUnlockEnemy();
            }
        }
    }

    private Enemy GetBestSuitableLockEnemy()
    {
        List<Enemy> spawnedEnemies;
        GameManager.instance.enemyManager.GetSpawnedEnemies(out spawnedEnemies);

        Enemy bestSuitableEnemy = null;
        float bestSuitableEnemyPriority = 0f;

        for (int i = 0; i < spawnedEnemies.Count; ++i)
        {
            float currentEnemyPriority = 0f;
            Vector3 currentEnemyPosition = spawnedEnemies[i].transform.position;

            currentEnemyPriority += ComputeDistancePriority(currentEnemyPosition);
            currentEnemyPriority += ComputeFrontOfPlayerPriority(currentEnemyPosition);

            if (currentEnemyPriority > bestSuitableEnemyPriority)
            {
                bestSuitableEnemyPriority = currentEnemyPriority;
                bestSuitableEnemy = spawnedEnemies[i];
            }
        }

        return bestSuitableEnemy;
    }

    private float ComputeDistancePriority(Vector3 enemyPosition)
    {
        float minSoftLockDistanceSqr = minSoftLockDistance * minSoftLockDistance;
        float currentSqrDistance = Vector3.SqrMagnitude(enemyPosition - transform.position);

        if (currentSqrDistance <= minSoftLockDistanceSqr)
        {
            return 1f - currentSqrDistance / minSoftLockDistanceSqr;
        }

        return 0f;
    }

    private float ComputeFrontOfPlayerPriority(Vector3 enemyPosition)
    {
        float playerFrontAngleToEnemyAngle = Vector3.Angle(transform.forward, enemyPosition - transform.position);
        if (playerFrontAngleToEnemyAngle <= maxAngleFrontOfPlayer)
        {
            return frontOfPlayerAddedPriority;
        }

        return 0f;
    }

    private Enemy GetNextEnemy(Enemy enemy)
    {
        List<Enemy> spawnedEnemies;
        GameManager.instance.enemyManager.GetSpawnedEnemies(out spawnedEnemies);

        int currentEnemyIndex = -1;

        for (int i = 0; i < spawnedEnemies.Count; ++i)
        {
            if (spawnedEnemies[i] == enemy)
            {
                currentEnemyIndex = i;
                break;
            }
        }

        if (currentEnemyIndex == -1)
        {
            return null;
        }
        int nextEnemyIndex = (currentEnemyIndex + 1) % spawnedEnemies.Count;
        return spawnedEnemies[nextEnemyIndex];
    }

}
