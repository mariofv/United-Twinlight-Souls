using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshHelper
{
    public static Vector3 FindAvailableSpawnPosition()
    {
        Vector3 playerPosition = GameManager.instance.player.Character().characterMovementManager.GetPosition();
        Vector2 randomOffset = Random.insideUnitCircle * 5f;
        Vector3 randomPoint = playerPosition + new Vector3(randomOffset.x, 0f, randomOffset.y);

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPoint, out navMeshHit, 2f, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            throw new UnityException("Cannot spawn enemy because there is no suitable point near player!");
        }
    }
}
