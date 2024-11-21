using MazeGenerator;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(CellRule))]
    public class CustomCellRuleInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty top = serializedObject.FindProperty("Top");
            SerializedProperty right = serializedObject.FindProperty("Right");
            SerializedProperty bottom = serializedObject.FindProperty("Bottom");
            SerializedProperty left = serializedObject.FindProperty("Left");
            SerializedProperty prefab = serializedObject.FindProperty("Prefab");

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Toggle("Top", top.boolValue);
                EditorGUILayout.Toggle("Right", right.boolValue);
                EditorGUILayout.Toggle("Bottom", bottom.boolValue);
                EditorGUILayout.Toggle("Left", left.boolValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(prefab);

            serializedObject.ApplyModifiedProperties();
        }
    }
}