using System;
using System.Collections.Generic;
using Maze;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Props))] [CanEditMultipleObjects]
    public class PropsCustomInspector : UnityEditor.Editor
    {
        private SerializedProperty _propType;
        private SerializedProperty _useCustomProbabilities;
        private SerializedProperty _globalProbabilityPerSlot;

        private SerializedProperty _barrelPrefabs;
        private SerializedProperty _barrelProbability;
        private SerializedProperty _cratesPrefabs;
        private SerializedProperty _cratesProbability;
        private SerializedProperty _bannerPrefabs;
        private SerializedProperty _bannerProbability;
        private SerializedProperty _rubblePrefabs;
        private SerializedProperty _rubbleProbability;
        // private SerializedProperty _brazierColumnPrefabs;
        // private SerializedProperty _brazierColumnProbability;

        private float _spaceBetweenTypes = 15;

        private void OnEnable()
        {
            try
            {
                _propType = serializedObject.FindProperty("_propType");
                _useCustomProbabilities = serializedObject.FindProperty("_useCustomProbabilities");
                _globalProbabilityPerSlot = serializedObject.FindProperty("_globalProbabilityPerSlot");

                _barrelPrefabs = serializedObject.FindProperty("_barrelPrefabs");
                _barrelProbability = serializedObject.FindProperty("_barrelProbability");
                _cratesPrefabs = serializedObject.FindProperty("_cratesPrefabs");
                _cratesProbability = serializedObject.FindProperty("_cratesProbability");
                _bannerPrefabs = serializedObject.FindProperty("_bannerPrefabs");
                _bannerProbability = serializedObject.FindProperty("_bannerProbability");
                _rubblePrefabs = serializedObject.FindProperty("_rubblePrefabs");
                _rubbleProbability = serializedObject.FindProperty("_rubbleProbability");
                // _brazierColumnPrefabs = serializedObject.FindProperty("_brazierColumnPrefabs");
                // _brazierColumnProbability = serializedObject.FindProperty("_brazierColumnProbability");
            }
            catch
            {
                Debug.LogWarning("Unknown error while recovering serialized properties");
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _propType.intValue = EditorGUILayout.MaskField("Type", _propType.intValue, _propType.enumDisplayNames);

            EditorGUILayout.PropertyField(_useCustomProbabilities);

            if (!_useCustomProbabilities.boolValue)
            {
                _globalProbabilityPerSlot.floatValue =  EditorGUILayout.Slider("Global Probability Per Slot", _globalProbabilityPerSlot.floatValue, 0, 1);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (_propType.intValue.Equals(0)) return;

            Props.Type type = (Props.Type)_propType.intValue;

            EditorGUILayout.Space(5);

            if (_useCustomProbabilities.boolValue)
            {
                EditorGUILayout.HelpBox("Custom probabilities are enabled.\nThe sum of all probabilities must be between 0 and 1.", MessageType.Warning);
                EditorGUILayout.Space(20);
            }

            if (type.HasFlag(Props.Type.Barrel))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_barrelPrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_barrelProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_barrelPrefabs);
                }

                EditorGUILayout.Space(_spaceBetweenTypes);
            }

            if (type.HasFlag(Props.Type.Crates))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_cratesPrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_cratesProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_cratesPrefabs);
                }

                EditorGUILayout.Space(_spaceBetweenTypes);
            }

            if (type.HasFlag(Props.Type.Banner))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_bannerPrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_bannerProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_bannerPrefabs);
                }

                EditorGUILayout.Space(_spaceBetweenTypes);
            }

            if (type.HasFlag(Props.Type.Rubble))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_rubblePrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_rubbleProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_rubblePrefabs);
                }
            }

            /* if (type.HasFlag(Props.Type.BrazierColumn))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(_brazierColumnPrefabs);
                        EditorGUILayout.Space(10);
                        EditorGUILayout.PropertyField(_brazierColumnProbability);
                    GUILayout.EndHorizontal();
                }
                else {
                    EditorGUILayout.PropertyField(_brazierColumnPrefabs);
                }
            } */

            UpdateDictionary();

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateDictionary()
        {
            Props props = (Props)target;

            props.Prefabs = new()
            {
                { "Barrel", props.BarrelPrefabs },
                { "Crates", props.CratesPrefabs },
                { "Banner", props.BannerPrefabs },
                { "Rubble", props.RubblePrefabs },
                // { "BrazierColumn", props.BrazierColumnPrefabs }
            };
        }
    }
}