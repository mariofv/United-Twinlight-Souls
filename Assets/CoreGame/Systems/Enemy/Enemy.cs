using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private NavMeshAgent enemyNavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyNavMeshAgent.SetDestination(GameManager.instance.player.Character().characterMovementManager.GetPosition());
    }
}