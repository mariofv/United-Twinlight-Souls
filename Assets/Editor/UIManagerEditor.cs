using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager), true)]
public class UIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIManager uiManager = target as UIManager;
        SceneAsset oldMainMenuUIScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(uiManager.mainMenuScene);
        SceneAsset oldGameUIScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(uiManager.gameUIScene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelTransitionUIManager"));

        EditorGUILayout.LabelField("Main Menu UI");
        SceneAsset newMainMenuUIScene = EditorGUILayout.ObjectField("Main Menu UI Scene", oldMainMenuUIScene, typeof(SceneAsset), false) as SceneAsset;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mainMenuUIManager"));

        EditorGUILayout.LabelField("Game UI");
        SceneAsset newGameUIScene = EditorGUILayout.ObjectField("Game UI Scene", oldGameUIScene, typeof(SceneAsset), false) as SceneAsset;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameUIManager"));

        if (EditorGUI.EndChangeCheck())
        {
            string newMainMenuUIScenePath = AssetDatabase.GetAssetPath(newMainMenuUIScene);
            SerializedProperty mainMenuUIScenePathProperty = serializedObject.FindProperty("mainMenuScene");
            mainMenuUIScenePathProperty.stringValue = newMainMenuUIScenePath;

            string newGameUIScenePath = AssetDatabase.GetAssetPath(newGameUIScene);
            SerializedProperty gameUIScenePathProperty = serializedObject.FindProperty("gameUIScene");
            gameUIScenePathProperty.stringValue = newGameUIScenePath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
