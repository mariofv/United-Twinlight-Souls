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
            GameManager.instance.player.GetControlledCharacter().DisableMovement();
            GameManager.instance.player.GetBarald().Teleport(GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition());
            GameManager.instance.player.ControlBarald();
            GameManager.instance.player.GetControlledCharacter().EnableMovement();
            GameManager.instance.uiManager.gameUIManager.hudUI.SelectBaraldPortrait();
        }
        if (GUILayout.Button("Control Ilona"))
        {
            GameManager.instance.player.GetControlledCharacter().DisableMovement();
            GameManager.instance.player.GetIlona().Teleport(GameManager.instance.player.GetControlledCharacter().characterMovementManager.GetPosition());
            GameManager.instance.player.ControlIlona();
            GameManager.instance.player.GetControlledCharacter().EnableMovement();
            GameManager.instance.uiManager.gameUIManager.hudUI.SelectIlonaPortrait();
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Hurt (10)"))
        {
            GameManager.instance.player.GetControlledCharacter().characterStatsManager.Hurt(10);
        }
        GUILayout.EndHorizontal();
        
        
        GUILayout.EndVertical();
    }
}
