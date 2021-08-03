using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Binding
{

    [CustomEditor(typeof(MultiBinding), true)]
    public class MultiBindingEditor : Editor
    {
        private Type[] _Converters;
        private string[] _TypeNames;

        void OnEnable()
        {
            var converterType = typeof(MultiValueConverter);
            _Converters = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => converterType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();

            _TypeNames = new string[_Converters.Count()];
            for (int i = 0; i < _Converters.Length; i++) _TypeNames[i] = _Converters[i].Name;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            //EditorGUILayout.Separator();

            var binding = target as MultiBinding;
            var subBindings = serializedObject.FindProperty(nameof(MultiBinding.Bindings));
            EditorGUILayout.PropertyField(subBindings,true);


            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MultiBinding.Target)));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(MultiBinding.TargetPath)), GUILayout.ExpandWidth(true));

            var targetPaths = new List<string>();
            if (binding.Target != null)
            {
                var targetType = binding.Target.GetType();
                var members = targetType.GetMembers();
                foreach (var member in members)
                {
                    if ((member is FieldInfo fInfo && !fInfo.IsInitOnly) || (member is PropertyInfo pInfo && pInfo.CanWrite))
                    {
                        targetPaths.Add(member.Name);
                    }
                }
            }
            var pathsArr = targetPaths.ToArray();
            int pathIndex = EditorGUILayout.Popup(-1, pathsArr, GUILayout.Width(20));
            if (pathIndex >= 0)
            {
                binding.TargetPath = targetPaths[pathIndex];
            }
            EditorGUILayout.EndHorizontal();

            // Converters:
            Type oldType = binding.Converter?.GetType();
            int index = Array.IndexOf(_Converters, oldType ?? typeof(DefaultMultiConverter));
            index = EditorGUILayout.Popup("Converter", index, _TypeNames);
            Type newType = _Converters[index];
            if (newType != oldType)
            {
                if (binding.Converter != null)
                {
                    AssetDatabase.RemoveObjectFromAsset(binding.Converter);
                }
                //var resources = Resources.FindObjectsOfTypeAll(newType);
                var converter = CreateInstance(newType);
                var stage = PrefabStageUtility.GetCurrentPrefabStage();
                if (stage != null)
                {
                    AssetDatabase.AddObjectToAsset(converter, stage.assetPath);
                }

                binding.Converter = converter as MultiValueConverter;

            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {

                EditorUtility.SetDirty(binding);
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                    var prefab = prefabStage.prefabContentsRoot;
                    EditorUtility.SetDirty(prefab);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(prefab);
                }
                
            }
        }
    }

}