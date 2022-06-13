using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent enemyNavMeshAgent;
    [SerializeField] protected PlayerDetectionCollider playerDetectionCollider;

    protected Enemy enemy;
    [SerializeField] protected AudioSource audioSource;

    private float currentBaseTime = 0f;
    protected float deltaTimeAI;
    private const float AI_UPDATE_TIME = 0.1f;

    private void Update()
    {
        currentBaseTime += Time.deltaTime;
        if (currentBaseTime >= AI_UPDATE_TIME)
        {
            deltaTimeAI = currentBaseTime;
            UpdateAI();
            currentBaseTime = 0f;
        }
        UpdateSpecific();
    }

    protected abstract void UpdateAI();
    protected virtual void UpdateSpecific() { }
    public abstract void Reanimate();
    public abstract void OnSpawnStart();
    
    public void OnSpawnEnd()
    {
        OnSpawnEndSpecific();
        enemy.onSpawnEnd.Invoke(enemy);
    }

    public virtual void OnSpawnEndSpecific() { }
    public abstract void OnDeathStart();
    public abstract void OnHitStart();

    public void Link(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void SpawnInNavMesh(Vector3 position)
    {
        enemyNavMeshAgent.Warp(position);
        EnableNavMeshAgent();
    }

    public void EnableNavMeshAgent()
    {
        enemyNavMeshAgent.enabled = true;
    }

    protected void DisableNavMeshAgent()
    {
        enemyNavMeshAgent.enabled = false;
    }

    public bool IsAgentMoving()
    {
        return enemyNavMeshAgent.velocity != Vector3.zero;
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }
}
