using Lighting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace Editor.CustomInspectors
{

[CustomEditor(typeof(EmitterComposite))]
public class EmitterCompositeInspector : UnityEditor.Editor
{
    private SerializedProperty _lights;
    private SerializedProperty _emitters;

    private void OnEnable()
    {
        _lights = serializedObject.FindProperty("_lights");
        _emitters = serializedObject.FindProperty("_emitters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Dynamic runtime emitters", new GUIStyle { fontStyle = FontStyle.Bold, normal = { textColor = Color.white }});

        DropAreaGUI(CreateEmitterField, "Drag components here", 40, new Color(.2f, .2f, .2f, .25f));

        EditorGUILayout.PropertyField(_lights, true);
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(_emitters, true);

        serializedObject.ApplyModifiedProperties();
    }

    public void DropAreaGUI(System.Action<Object> dragHandler, string boxLabel, float boxHeight, Color boxBackgroundColor)
    {
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, boxHeight, GUILayout.ExpandWidth(true));
        Texture2D boxBackground = new(1, 1, TextureFormat.RGBAFloat, false);
        boxBackground.SetPixel(0, 0, boxBackgroundColor);
        boxBackground.Apply();

        GUI.Box(dropArea, boxLabel, new GUIStyle { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white, background = boxBackground } });

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        dragHandler(draggedObject);
                    }
                }
                break;
        }
    }

    public void CreateEmitterField(Object item)
    {
        if (item is GameObject gameObject)
        {
            Light light = gameObject.GetComponent<Light>();
            ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
            VisualEffect visualEffect = gameObject.GetComponent<VisualEffect>();

            if (light)
            {
                AddLight(new LightEmitter(light));
            }
            else if (particleSystem)
            {
                AddEmitter(new EffectEmitter(particleSystem));
            }
            else if (visualEffect)
            {
                AddEmitter(new EffectEmitter(visualEffect));
            }
        }
    }

    private void AddLight(LightEmitter lightEmitter)
    {
        serializedObject.Update();
        _lights.arraySize++;
        SerializedProperty newLight = _lights.GetArrayElementAtIndex(_lights.arraySize - 1);
        newLight.FindPropertyRelative("Light").objectReferenceValue = lightEmitter.Light;
        newLight.FindPropertyRelative("IsFlickering").boolValue = lightEmitter.IsFlickering;
        newLight.FindPropertyRelative("FlickerDuration").floatValue = lightEmitter.FlickerDuration;
        newLight.FindPropertyRelative("FlickerCurve").animationCurveValue = lightEmitter.FlickerCurve;
        serializedObject.ApplyModifiedProperties();
    }

    private void AddEmitter(EffectEmitter emitter)
    {
        serializedObject.Update();
        _emitters.arraySize++;
        SerializedProperty newEmitter = _emitters.GetArrayElementAtIndex(_emitters.arraySize - 1);
        newEmitter.FindPropertyRelative("System").enumValueIndex = (int)emitter.System;
        newEmitter.FindPropertyRelative("PS").objectReferenceValue = emitter.PS;
        newEmitter.FindPropertyRelative("VFX").objectReferenceValue = emitter.VFX;
        serializedObject.ApplyModifiedProperties();
    }
}
}