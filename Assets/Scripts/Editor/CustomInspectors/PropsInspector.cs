using Maze;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(Props))] [CanEditMultipleObjects]
    public class PropsInspector : UnityEditor.Editor
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
        private SerializedProperty _spiderWebsPrefabs;
        private SerializedProperty _spiderWebsProbability;
        private SerializedProperty _rugPrefabs;
        private SerializedProperty _rugProbability;
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
                _spiderWebsPrefabs = serializedObject.FindProperty("_spiderWebsPrefabs");
                _spiderWebsProbability = serializedObject.FindProperty("_spiderWebsProbability");
                _rugPrefabs = serializedObject.FindProperty("_rugPrefabs");
                _rugProbability = serializedObject.FindProperty("_rugProbability");
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

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (_propType.intValue.Equals(0)) return;

            Props.Type type = (Props.Type)_propType.intValue;

            EditorGUILayout.Space(5);

            if (_useCustomProbabilities.boolValue)
            {
                EditorGUILayout.HelpBox(
                    "Custom probabilities are enabled.\n" +
                    "Each props types have custom probabilities to be generated which influence the generation. Object spawn follow type-order priority.\n" +
                    "Probabilities must be between 0 and 1.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "Global probability per slot is currently in-use.\n" +
                    "Each props type have the same probability to spawn.\n" +
                    "Current value is hardcoded to 0.5", MessageType.Info);
            }

            EditorGUILayout.Space(10);

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
                
                EditorGUILayout.Space(_spaceBetweenTypes);
            }
            
            if (type.HasFlag(Props.Type.SpiderWebs))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_spiderWebsPrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_spiderWebsProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_spiderWebsPrefabs);
                }
                
                EditorGUILayout.Space(_spaceBetweenTypes);
            }
            
            if (type.HasFlag(Props.Type.Rug))
            {
                if (_useCustomProbabilities.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_rugPrefabs);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(_rugProbability);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(_rugPrefabs);
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
                { "Barrel", (props.BarrelPrefabs, props.BarrelProbability) },
                { "Crates", (props.CratesPrefabs, props.CratesProbability) },
                { "Banner", (props.BannerPrefabs, props.BannerProbability) },
                { "Rubble", (props.RubblePrefabs, props.RubbleProbability) },
                // { "BrazierColumn", props.BrazierColumnPrefabs }
            };
        }
    }
}