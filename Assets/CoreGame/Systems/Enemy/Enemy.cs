using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private List<Collider> enemyHurtBoxes;
    [SerializeField] private bool isBoss;

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("Hit reaction")]
    [SerializeField] private bool orientateEnemyAfterHit;

    [Header("Loot")]
    [SerializeField] private int minAmountLightOrbs;
    [SerializeField] private int maxAmountLightOrbs;

    public UnityEvent onSpawnedEnemyDead;

    void Awake()
    {
        enemyAI.Link(this);
    }

    public void Hurt(int damage)
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.saitamaMode)
            {
                damage = 9999;
            }
        }

        Vector3 enemyPosition = transform.position;
        Vector3 attackerPosition = GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition();
        Vector3 attackDirection = (enemyPosition - attackerPosition).normalized;
        attackDirection.y = 0f;

        if (orientateEnemyAfterHit)
        {
            transform.rotation = Quaternion.LookRotation(-attackDirection);
        }

        GameManager.instance.uiManager.gameUIManager.damageIndicatorUI.ShowDamageIndicator(damage, attackerPosition, enemyPosition);

        int previousHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (isBoss)
        {
            float currentPercentage = ((float)currentHealth) / maxHealth;
            float previousPercentage = ((float)previousHealth) / maxHealth;
            GameManager.instance.uiManager.gameUIManager.bossHealthBarUI.InflictDamage(currentPercentage, previousPercentage);
            GameManager.instance.levelManager.GetCurrentLevelAsBoss().CheckPhaseTransition(currentPercentage);
        }

        if (currentHealth <= 0)
        {
            SetInvincible(true);
            enemyAI.OnDeathStart();
        }
        else
        {
            enemyAI.OnHitStart();
        }
    }

    public void Reanimate()
    {
        onSpawnedEnemyDead.RemoveAllListeners();
        currentHealth = maxHealth;
        SetInvincible(false);
        gameObject.SetActive(true);
        enemyAI.Reanimate();
    }

    public void Kill(bool spawnLoot = true)
    {
        if (spawnLoot)
        {
            SpawnLoot();
        }

        gameObject.SetActive(false);
        GameManager.instance.enemyManager.RemoveSpawnedEnemy(this);
        onSpawnedEnemyDead.Invoke();
    }

    public void Spawn(Vector3 position, bool spawnAnimation)
    {
        enemyAI.SpawnInNavMesh(position);
        Reanimate();

        if (spawnAnimation)
        {
            enemyAI.OnSpawnStart();
        }
    }

    private void SpawnLoot()
    {
        int numberOfSpawnedLightOrbs = Random.Range(minAmountLightOrbs, maxAmountLightOrbs + 1);
        GameManager.instance.lootManager.SpawnLightOrbs(transform.position, numberOfSpawnedLightOrbs);
    }

    public bool IsAlive()
    {
        return gameObject.activeSelf;
    }

    public void SetAnimatorBool(string boolName, bool value)
    {
        enemyAnimator.SetBool(boolName, value);
    }
    
    public void TriggerAnimation(string triggerName)
    {
        enemyAnimator.SetTrigger(triggerName);
    }

    public void SetAnimatorSpeed(float speed)
    {
        enemyAnimator.speed = speed;
    }

    public void SetInvincible(bool isInvincible)
    {
        for (int i = 0; i < enemyHurtBoxes.Count; ++i)
        {
            enemyHurtBoxes[i].enabled = !isInvincible;
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentHealthPercentage()
    {
        return ((float)currentHealth) / maxHealth;
    }
}
