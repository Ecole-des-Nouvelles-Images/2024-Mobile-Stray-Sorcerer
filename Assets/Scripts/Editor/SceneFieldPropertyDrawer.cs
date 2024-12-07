using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty sceneAsset = property.FindPropertyRelative("_sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("_sceneName");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (sceneAsset != null)
            {
                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

                if (sceneAsset.objectReferenceValue)
                {
                    sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset)?.name;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}