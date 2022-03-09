using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebugMenu : DebugMenu
{
    void OnGUI()
    {
        GUILayout.Window((int)DebugMenu.DebugMenuId.CHARACTERS, new Rect(10, 10, 400, 300), WindowFunction, "Enemy Debug Menu");
    }

    private void WindowFunction(int windowID)
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Spawn Mushdoom"))
        {
            GameManager.instance.enemyManager.SpawnEnemy(EnemyAsset.EnemyId.MUSHDOOM);
        }
        if (GUILayout.Button("Spawn Bitter"))
        {
            GameManager.instance.enemyManager.SpawnEnemy(EnemyAsset.EnemyId.BITTER);
        }
        if (GUILayout.Button("Spawn Necroplant"))
        {
            GameManager.instance.enemyManager.SpawnEnemy(EnemyAsset.EnemyId.NECROPLANT);
        }

        GUILayout.EndVertical();
    }
}
