using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebugMenu : DebugMenu
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        GUILayout.EndVertical();

        if (GUILayout.Button(new GUIContent("Set camera order default")))
        {
            Camera.main.transparencySortMode = TransparencySortMode.Default;

        }
        if (GUILayout.Button(new GUIContent("Set camera order perspective")))
        {
            Camera.main.transparencySortMode = TransparencySortMode.Perspective;

        }
        if (GUILayout.Button(new GUIContent("Set camera order ortographic")))
        {
            Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        }
        if (GUILayout.Button(new GUIContent("Set camera order ortographic")))
        {
            Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;

        }
    }
}
