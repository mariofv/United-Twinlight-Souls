using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent enemyNavMeshAgent;
    [SerializeField] protected PlayerDetectionCollider playerDetectionCollider;

    protected Enemy enemy;

    private float currentTime = 0f;
    protected float deltaTimeAI;
    private const float AI_UPDATE_TIME = 0.1f;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= AI_UPDATE_TIME)
        {
            deltaTimeAI = currentTime;
            UpdateAI();
            currentTime = 0f;
        }
    }

    protected abstract void UpdateAI();
    public abstract void Reanimate();
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
}
