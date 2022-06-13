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
    private EnemyHurtbox currentSoftLockedEnemyHurtbox = null;

    private EnemyHurtbox currentLockedEnemyHurtbox = null;

    // Update is called once per frame
    void Update()
    {
        if (currentState != LockState.LOCK)
        {
            currentSoftLockCheckTime += Time.deltaTime;
            if (currentSoftLockCheckTime >= timeUntilSoftLockCheck)
            {
                currentSoftLockCheckTime = 0f;
                CheckPotentialLockedEnemyHurtbox();
            }
        }
    }

    public void SwitchLockEnemyHurtbox()
    {
        if (currentState != LockState.LOCK)
        {
            EnemyHurtbox softLockedEnemyHurtbox = currentSoftLockedEnemyHurtbox;
            if (softLockedEnemyHurtbox == null)
            {
                return;
            }

            SoftUnlockEnemyHurtbox();
            LockEnemyHurtbox(softLockedEnemyHurtbox);
        }
        else
        {
            UnlockEnemyHurtbox();
        }
    }

    public void ChangeLockedEnemy()
    {
        if (currentState != LockState.LOCK)
        {
            return;
        }

        EnemyHurtbox nextEnemyHurtbox = GetNextEnemyHurtbox(currentLockedEnemyHurtbox);
        if (nextEnemyHurtbox != null)
        {
            UnlockEnemyHurtbox(hasToHideUI: false);
            LockEnemyHurtbox(nextEnemyHurtbox);
        }
    }

    private void SoftLockEnemyHurtbox(EnemyHurtbox enemyHurtbox)
    {
        currentSoftLockedEnemyHurtbox = enemyHurtbox;
        currentSoftLockedEnemyHurtbox.GetEnemyScript().onSpawnedEnemyDead.AddListener(OnSoftLockedEnemyDeath);

        GameManager.instance.uiManager.gameUIManager.lockUI.SoftLockEnemy(currentSoftLockedEnemyHurtbox.transform);
        currentState = LockState.SOFT_LOCK;
    }

    private void SoftUnlockEnemyHurtbox()
    {
        currentSoftLockedEnemyHurtbox.GetEnemyScript().onSpawnedEnemyDead.RemoveListener(OnSoftLockedEnemyDeath);
        currentSoftLockedEnemyHurtbox = null;

        GameManager.instance.uiManager.gameUIManager.lockUI.UnlockEnemy(hasToHideUI: true);
        currentState = LockState.NONE;
    }

    private void OnSoftLockedEnemyDeath(Enemy enemy)
    {
        SoftUnlockEnemyHurtbox();
    }

    private void LockEnemyHurtbox(EnemyHurtbox enemyHurtbox)
    {
        currentLockedEnemyHurtbox = enemyHurtbox;
        currentLockedEnemyHurtbox.GetEnemyScript().onSpawnedEnemyDead.AddListener(OnLockedEnemyDeath);

        GameManager.instance.uiManager.gameUIManager.lockUI.LockEnemy(currentLockedEnemyHurtbox.transform);
        currentState = LockState.LOCK;
    }

    private void UnlockEnemyHurtbox(bool hasToHideUI = true)
    {
        currentLockedEnemyHurtbox.GetEnemyScript().onSpawnedEnemyDead.RemoveListener(OnLockedEnemyDeath);
        currentLockedEnemyHurtbox = null;

        GameManager.instance.uiManager.gameUIManager.lockUI.UnlockEnemy(hasToHideUI);
        currentState = LockState.NONE;
    }

    private void OnLockedEnemyDeath(Enemy enemy)
    {
        UnlockEnemyHurtbox();
        EnemyHurtbox bestSuitableEnemyHurtobx = GetBestSuitableLockEnemyHurtbox();
        if (bestSuitableEnemyHurtobx != null)
        {
            LockEnemyHurtbox(bestSuitableEnemyHurtobx);
        }
    }

    private void CheckPotentialLockedEnemyHurtbox()
    {
        EnemyHurtbox bestSuitableEnemyHurtbox = GetBestSuitableLockEnemyHurtbox();
        if (bestSuitableEnemyHurtbox != null)
        {
            if (currentState == LockState.SOFT_LOCK)
            {
                if (bestSuitableEnemyHurtbox != currentSoftLockedEnemyHurtbox)
                {
                    SoftUnlockEnemyHurtbox();
                    SoftLockEnemyHurtbox(bestSuitableEnemyHurtbox);
                }
            }
            else
            {
                SoftLockEnemyHurtbox(bestSuitableEnemyHurtbox);
            }
        }
        else
        {
            if (currentState == LockState.SOFT_LOCK)
            {
                SoftUnlockEnemyHurtbox();
            }
        }
    }

    private EnemyHurtbox GetBestSuitableLockEnemyHurtbox()
    {
        List<Enemy> spawnedEnemies;
        GameManager.instance.enemyManager.GetSpawnedEnemies(out spawnedEnemies);

        EnemyHurtbox bestSuitableEnemyHurtbox = null;
        float bestSuitableEnemyPriority = 0f;

        for (int i = 0; i < spawnedEnemies.Count; ++i)
        {
            for (int j = 0; j < spawnedEnemies[i].enemyHurtBoxes.Count; ++j)
            {
                float currentEnemyHurtboxPriority = 0f;
                Vector3 currentEnemyHurtboxPosition = spawnedEnemies[i].enemyHurtBoxes[j].transform.position;

                currentEnemyHurtboxPriority += ComputeDistancePriority(currentEnemyHurtboxPosition);
                currentEnemyHurtboxPriority += ComputeFrontOfPlayerPriority(currentEnemyHurtboxPosition);

                if (currentEnemyHurtboxPriority > bestSuitableEnemyPriority)
                {
                    bestSuitableEnemyPriority = currentEnemyHurtboxPriority;
                    bestSuitableEnemyHurtbox = spawnedEnemies[i].enemyHurtBoxes[j];
                }

            }
        }

        return bestSuitableEnemyHurtbox;
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

    private EnemyHurtbox GetNextEnemyHurtbox(EnemyHurtbox enemyHurtbox)
    {
        List<Enemy> spawnedEnemies;
        GameManager.instance.enemyManager.GetSpawnedEnemies(out spawnedEnemies);
        List<EnemyHurtbox> enemiesHurtboxes = new List<EnemyHurtbox>();

        for (int i = 0; i < spawnedEnemies.Count; ++i)
        {
            for (int j = 0; j < spawnedEnemies[i].enemyHurtBoxes.Count; ++j)
            {
                enemiesHurtboxes.Add(spawnedEnemies[i].enemyHurtBoxes[j]);
            }
        }

        for (int i = 0; i < enemiesHurtboxes.Count; ++i)
        {
            if (enemiesHurtboxes[i] == enemyHurtbox)
            {
                return enemiesHurtboxes[(i + 1) % enemiesHurtboxes.Count];
            }
        }

        return null;
    }

    public bool IsLockingEnemy()
    {
        return currentState != LockState.NONE;
    }

    public Vector3 GetLockedEnemyHurtboxPosition()
    {
        if (currentState == LockState.LOCK)
        {
            return currentLockedEnemyHurtbox.transform.position;
        }
        else
        {
            return currentSoftLockedEnemyHurtbox.transform.position;
        }
    }

}
