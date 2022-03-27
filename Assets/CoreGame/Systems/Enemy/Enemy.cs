using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Animator enemyAnimator;

    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void Awake()
    {
        enemyAI.Link(this);
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("moving", enemyAI.IsAgentMoving());
    }

    public void Hurt(float damage)
    {
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        if (currentHealth == 0f)
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
