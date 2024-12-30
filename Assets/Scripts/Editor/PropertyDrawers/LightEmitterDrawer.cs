using Lighting;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(LightEmitter))]
    public class LightEmitterDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2; // Space between fields

            Rect lightRect = new Rect(position.x, position.y, position.width, lineHeight);
            EditorGUI.PropertyField(lightRect, property.FindPropertyRelative("Light"), new GUIContent("Light"));

            Rect flickerRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
            EditorGUI.PropertyField(flickerRect, property.FindPropertyRelative("IsFlickering"), new GUIContent("Is Flickering"));

            bool isFlickering = property.FindPropertyRelative("IsFlickering").boolValue;

            if (isFlickering)
            {
                Rect durationRect = new Rect(position.x, position.y + (lineHeight + spacing) * 2, position.width, lineHeight);
                EditorGUI.PropertyField(durationRect, property.FindPropertyRelative("FlickerDuration"), new GUIContent("Flicker Duration"));

                Rect curveRect = new Rect(position.x, position.y + (lineHeight + spacing) * 3, position.width, lineHeight);
                EditorGUI.PropertyField(curveRect, property.FindPropertyRelative("FlickerCurve"), new GUIContent("Flicker Curve"));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2; // Space between fields
            float extraSpacing = 10; // Extra space between list elements

            bool isFlickering = property.FindPropertyRelative("IsFlickering").boolValue;
            int fieldCount = isFlickering ? 4 : 2;

            return lineHeight * fieldCount + spacing * (fieldCount - 1) + extraSpacing;
        }
    }
}