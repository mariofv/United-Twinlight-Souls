using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tutorial))]
[CanEditMultipleObjects]
public class TutorialEditor : Editor
{
    SerializedProperty tutorialEvents;

    void OnEnable()
    {
        tutorialEvents = serializedObject.FindProperty("tutorialEvents");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(tutorialEvents);

        if (GUILayout.Button("Add ShowText event"))
        {
            ((Tutorial)serializedObject.targetObject).tutorialEvents.Add(new Tutorial.ShowTextTutorialEvent());
        }
        if (GUILayout.Button("Add SpawnEnemies event"))
        {
            ((Tutorial)serializedObject.targetObject).tutorialEvents.Add(new Tutorial.SpawnEnemyTutorialEvent());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
