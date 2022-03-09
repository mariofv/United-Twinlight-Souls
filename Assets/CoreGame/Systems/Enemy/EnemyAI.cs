using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent enemyNavMeshAgent;

    protected Enemy enemy;

    public void Link(Enemy enemy)
    {
        this.enemy = enemy;
    }

    // Update is called once per frame
    void Update()
    {
        enemyNavMeshAgent.SetDestination(GameManager.instance.player.Character().characterMovementManager.GetPosition());
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

    public void DisableNavMeshAgent()
    {
        enemyNavMeshAgent.enabled = false;
    }

    public bool IsAgentMoving()
    {
        return enemyNavMeshAgent.velocity != Vector3.zero;
    }
}
