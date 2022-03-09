using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Animator enemyAnimator;

    void Awake()
    {
        enemyAI.Link(this);        
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("moving", enemyAI.IsAgentMoving());
    }

    public void Reanimate()
    {
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
}
