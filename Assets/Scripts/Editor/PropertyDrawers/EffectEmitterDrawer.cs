using Lighting;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(EffectEmitter))]
    public class EffectEmitterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2;

            Rect typeRect = new Rect(position.x, position.y, position.width, lineHeight);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("System"), new GUIContent("Type"));

            Rect componentRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);

            if (property.FindPropertyRelative("System").enumValueIndex == (int)EffectEmitter.Type.ParticleSystem)
            {
                EditorGUI.PropertyField(componentRect, property.FindPropertyRelative("PS"), new GUIContent("Particle System"));
            }
            else
            {
                EditorGUI.PropertyField(componentRect, property.FindPropertyRelative("VFX"), new GUIContent("Visual Effect"));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2;
            float extraSpacing = 10;
            return lineHeight * 2 + spacing + extraSpacing;
        }
    }
}