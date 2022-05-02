using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;

    public Enemy GetEnemyScript()
    {
        return enemyScript;
    }
}
