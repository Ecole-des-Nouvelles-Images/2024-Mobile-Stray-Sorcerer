using Maze;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(CellRule))]
    public class CellRulePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty prefab = property.FindPropertyRelative("Prefab");

            SerializedProperty top = property.FindPropertyRelative("Top");
            SerializedProperty right = property.FindPropertyRelative("Right");
            SerializedProperty bottom = property.FindPropertyRelative("Bottom");
            SerializedProperty left = property.FindPropertyRelative("Left");

            // Reduce the indentation level
            EditorGUI.indentLevel = 0;

            // Adjust the starting position of the fields
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.x -= 50; // Adjust this value to move the fields closer to the label

            // Calculate the width of the columns
            float columnWidth = (position.width + 50) / 2;

            // Draw the GameObject field in the first column
            Rect gameObjectRect = new Rect(position.x, position.y, columnWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(gameObjectRect, prefab, GUIContent.none);

            // Calculate the total height needed for all fields
            float totalHeight = EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 4;

            // Draw the preview of the prefab in the second column over the entire height
            Rect previewRect = new Rect(position.x + columnWidth, position.y, columnWidth, totalHeight);
            if (prefab.objectReferenceValue != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(prefab.objectReferenceValue as GameObject);
                if (previewTexture)
                {
                    GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
                }
            }

            // Move to the next line
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Calculate the width of the sub-columns
            float subColumnWidth = columnWidth / 2;

            // Draw the labels and boolean fields in two sub-columns
            Rect labelRect = new Rect(position.x, position.y, subColumnWidth, EditorGUIUtility.singleLineHeight);
            Rect boolRect = new Rect(position.x + subColumnWidth - 20, position.y, subColumnWidth, EditorGUIUtility.singleLineHeight);

            // First row
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Top"));
            EditorGUI.PropertyField(boolRect, top, GUIContent.none);

            labelRect.x += subColumnWidth;
            boolRect.x += subColumnWidth;
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Right"));
            EditorGUI.PropertyField(boolRect, right, GUIContent.none);

            // Move to the next line
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            labelRect.y = position.y;
            boolRect.y = position.y;

            // Second row
            labelRect.x = position.x;
            boolRect.x = position.x + subColumnWidth - 20;
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Bottom"));
            EditorGUI.PropertyField(boolRect, bottom, GUIContent.none);

            labelRect.x += subColumnWidth;
            boolRect.x += subColumnWidth;
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Left"));
            EditorGUI.PropertyField(boolRect, left, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Return the total height needed to draw all fields
            return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 4;
        }
    }
}