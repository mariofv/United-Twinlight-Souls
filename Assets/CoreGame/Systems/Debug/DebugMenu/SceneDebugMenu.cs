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
                StartCoroutine(GameManager.instance.levelManager.LoadLevel(loadedLevel, waitLoadingScreenTime: false));
            }
        }
        GUILayout.EndVertical();


        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
        GUILayout.Label("Teleport to combat area");

        int loadedZone = -1;
        for (int i = 0; i < GameManager.instance.levelManager.GetCurrentLevel().GetNumberOfZones(); ++i)
        {
            if (GUILayout.Button("Combat area " + (i + 1)))
            {
                loadedZone = i;
            }
        }

        if (loadedZone != -1)
        {
            Vector3 zonePosition = GameManager.instance.levelManager.GetCurrentLevel().GetZonePosition(loadedZone);
            GameManager.instance.player.GetControlledCharacter().Teleport(zonePosition + Vector3.up * 2);
        }
        GUILayout.EndVertical();


        GUILayout.EndVertical();
    }
}
