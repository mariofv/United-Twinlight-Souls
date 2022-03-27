using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Animator enemyAnimator;

    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

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
        enemyAI.Reanimate();
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        enemyAI.DisableNavMeshAgent();
        gameObject.SetActive(false);
    }

    public void Spawn(Vector3 position)
    {
        enemyAI.SpawnInNavMesh(position);
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
