using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ControllerButtonImage))]
public class ControllerButtonImageEditor : ImageEditor
{
    SerializedProperty pcImageProperty;
    SerializedProperty psImageProperty;
    SerializedProperty xboxImageProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        pcImageProperty = serializedObject.FindProperty("pcImage");
        psImageProperty = serializedObject.FindProperty("psImage");
        xboxImageProperty = serializedObject.FindProperty("xboxImage");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(pcImageProperty);
        EditorGUILayout.PropertyField(psImageProperty);
        EditorGUILayout.PropertyField(xboxImageProperty);
        serializedObject.ApplyModifiedProperties();
    }
}