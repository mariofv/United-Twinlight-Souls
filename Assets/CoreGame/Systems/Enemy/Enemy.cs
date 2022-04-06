using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Animator enemyAnimator;

    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public UnityEvent onSpawnedEnemyDead;

    void Awake()
    {
        enemyAI.Link(this);
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("moving", enemyAI.IsAgentMoving());
    }

    public void Hurt(int damage)
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.godMode)
            {
                damage = 9999;
            }
        }

        Vector3 enemyPosition = transform.position;
        Vector3 attackerPosition = GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition();
        GameManager.instance.uiManager.gameUIManager.damageIndicatorUI.ShowDamageIndicator(damage, attackerPosition, enemyPosition);

        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth == 0)
        {
            Kill();
        }
    }

    public void Reanimate()
    {
        onSpawnedEnemyDead.RemoveAllListeners();
        enemyAI.Reanimate();
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        enemyAI.DisableNavMeshAgent();
        gameObject.SetActive(false);
        onSpawnedEnemyDead.Invoke();
    }

    public void Spawn(Vector3 position, bool spawnAnimation)
    {
        enemyAI.SpawnInNavMesh(position);
        Reanimate();

        if (spawnAnimation)
        {
            TriggerAnimation("spawn");
        }
    }

    public bool IsAlive()
    {
        return gameObject.activeSelf;
    }

    public void TriggerAnimation(string triggerName)
    {
        enemyAnimator.SetTrigger(triggerName);
    }
}
