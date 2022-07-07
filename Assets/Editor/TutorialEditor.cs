using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tutorial))]
[CanEditMultipleObjects]
public class TutorialEditor : Editor
{
    SerializedProperty tutorialEvents;
    SerializedProperty associatedProgression;

    void OnEnable()
    {
        tutorialEvents = serializedObject.FindProperty("tutorialEvents");
        associatedProgression = serializedObject.FindProperty("associatedProgression");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(associatedProgression);
        EditorGUILayout.PropertyField(tutorialEvents);

        if (GUILayout.Button("Add ShowText event"))
        {
            ((Tutorial)serializedObject.targetObject).tutorialEvents.Add(new Tutorial.ShowTextTutorialEvent());
        }
        if (GUILayout.Button("Add StartCombatArea event"))
        {
            ((Tutorial)serializedObject.targetObject).tutorialEvents.Add(new Tutorial.StartCombatAreaTutorialEvent());
        }
        if (GUILayout.Button("Add WaitTime event"))
        {
            ((Tutorial)serializedObject.targetObject).tutorialEvents.Add(new Tutorial.WaitTimeTutorialEvent());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
