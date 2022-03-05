using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDebugMenu : DebugMenu
{
    void OnGUI()
    {
        GUILayout.Window((int)DebugMenu.DebugMenuId.CHARACTERS, new Rect(10, 10, 400, 300), WindowFunction, "Character Debug Menu");
    }

    private void WindowFunction(int windowID)
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Control Barald"))
        {
            GameManager.instance.player.ControlBarald();
        }
        if (GUILayout.Button("Control Ilona"))
        {
            GameManager.instance.player.ControlIlona();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
