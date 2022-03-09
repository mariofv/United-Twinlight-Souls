using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;
    [SerializeField] private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyNavMeshAgent.SetDestination(GameManager.instance.player.Character().characterMovementManager.GetPosition());

        enemyAnimator.SetBool("moving", enemyNavMeshAgent.velocity != Vector3.zero);
    }

    public void Reanimate()
    {
        gameObject.SetActive(true);
    }

    public void Teleport(Vector3 position)
    {
        enemyNavMeshAgent.Warp(position);
    }

    public bool IsAlive()
    {
        return gameObject.activeSelf;
    }
}
