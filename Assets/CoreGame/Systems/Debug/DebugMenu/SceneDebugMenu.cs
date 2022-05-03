using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebugMenu : DebugMenu
{
    void OnGUI()
    {
        GUILayout.Window((int)DebugMenu.DebugMenuId.SCENES, new Rect(10, 10, 400, 300), WindowFunction, "Scenes Debug Menu");
    }

    private void WindowFunction(int windowID)
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Load Main Menu"))
        {
            GameManager.instance.InitMainMenu();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
        GUILayout.Label("Change Current Level");

        int loadedLevel = -1;
        for (int i = 0; i < 3; ++i)
        {
            if (GUILayout.Button("Load Level " + (i + 1)))
            {
                loadedLevel = i;
            }
        }

        if (loadedLevel != -1)
        {
            if (GameManager.instance.GetCurrentGameState() == GameManager.GameState.MAIN_MENU)
            {
                GameManager.instance.InitGame(loadedLevel);
            }
            else
            {
                GameManager.instance.levelManager.LoadLevel(loadedLevel, waitLoadingScreenTime: false);
            }
        }
        GUILayout.EndVertical();

        if (!GameManager.instance.levelManager.IsCurrentLevelBoss())
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            GUILayout.Label("Teleport to combat area");

            int loadedZone = -1;
            ZonedLevel currentLevel = GameManager.instance.levelManager.GetCurrentLevelAsZoned();
            for (int i = 0; i < currentLevel.GetNumberOfZones(); ++i)
            {
                if (GUILayout.Button("Combat area " + (i + 1)))
                {
                    loadedZone = i;
                }
            }

            if (loadedZone != -1)
            {
                Vector3 zonePosition = currentLevel.GetZonePosition(loadedZone);
                GameManager.instance.player.GetControlledCharacter().Teleport(zonePosition + Vector3.up * 2);
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            GUILayout.Label("Trigger boss phase");

            int bossPhase = -1;
            BossLevel currentLevel = GameManager.instance.levelManager.GetCurrentLevelAsBoss();
            for (int i = 1; i < currentLevel.GetNumberOfPhases(); ++i)
            {
                if (GUILayout.Button("Boss Phase " + (i + 1)))
                {
                    bossPhase = i;
                }
            }

            if (bossPhase != -1)
            {
                currentLevel.ForceStartBossPhase(bossPhase - 1);
            }
            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }
}
