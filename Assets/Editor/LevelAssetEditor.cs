using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level), true)]
public class LevelAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Level levelAsset = target as Level;
        SceneAsset oldLevelScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(levelAsset.levelScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelName"));

        SceneAsset newLevelScene = EditorGUILayout.ObjectField("Level Scene", oldLevelScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            string newLevelScenePath = AssetDatabase.GetAssetPath(newLevelScene);
            SerializedProperty levelSceneProperty = serializedObject.FindProperty("levelScene");
            levelSceneProperty.stringValue = newLevelScenePath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
