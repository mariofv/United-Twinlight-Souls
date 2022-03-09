using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;

    private bool isAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        enemyNavMeshAgent.SetDestination(GameManager.instance.player.Character().characterMovementManager.GetPosition());
    }

    public void Reanimate()
    {
        isAlive = true;
    }

    public void Teleport(Vector3 position)
    {
        enemyNavMeshAgent.Warp(position);
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
