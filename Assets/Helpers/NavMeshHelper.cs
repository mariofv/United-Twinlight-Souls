using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NavMeshHelper
{
    public static Vector3 FindAvailableSpawnPosition()
    {
        Vector3 playerPosition = GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition();
        return GetNearPosition(playerPosition, 5f);
    }

    public static Vector3 GetNearPosition(Vector3 position, float distance)
    {
        Vector2 randomOffset;
        Vector3 randomPoint;
        NavMeshHit navMeshHit;
        for (int i = 0; i < 30; ++i)
        {
            randomOffset = Random.insideUnitCircle * distance;
            randomPoint = position + new Vector3(randomOffset.x, 0f, randomOffset.y);

            if (NavMesh.SamplePosition(randomPoint, out navMeshHit, 2f, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }
        
        throw new UnityException("Cannot spawn enemy because there is no suitable point near player!");
    }
}
