using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Binding
{

    [CustomEditor(typeof(DataBinding), true)]
    public class DataBindingEditor : BindingEditorBase
    {
        private Type[] _Converters;
        private string[] _TypeNames;

        void OnEnable()
        {
            var converterType = typeof(ValueConverter);
            _Converters = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => converterType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();

            _TypeNames = new string[_Converters.Count()];
            for (int i = 0; i < _Converters.Length; i++) _TypeNames[i] = _Converters[i].Name;
        }

        /// <summary>
        /// Create basic property fields
        /// </summary>
        protected override void BasicProperties()
        {
            base.BasicProperties();


            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DataBinding.Target)));
            var targetPathProp = serializedObject.FindProperty(nameof(DataBinding.TargetPath));
            if (targetPathProp != null)
            {
                DataBinding dbinding = target as DataBinding;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(targetPathProp, GUILayout.ExpandWidth(true));

                var targetPaths = new List<string>();
                if (dbinding.Target != null)
                {
                    var targetType = dbinding.Target.GetType();
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
                    dbinding.TargetPath = targetPaths[pathIndex];
                }
                EditorGUILayout.EndHorizontal();
            }

            var binding = target as ConverterBindingBase;
            // Converters:
            Type oldType = binding.Converter?.GetType();
            int index = Array.IndexOf(_Converters, oldType ?? typeof(DefaultConverter));
            index = EditorGUILayout.Popup("Converter", index, _TypeNames);
            Type newType = _Converters[index];
            if (newType != oldType)
            {
                if (binding.Converter != null)
                {
                    AssetDatabase.RemoveObjectFromAsset(binding.Converter);
                }
                var converter = CreateInstance(newType);
                binding.Converter = converter as ValueConverter;
                AssetDatabase.AddObjectToAsset(converter, PrefabStageUtility.GetCurrentPrefabStage().assetPath);
            }

        }

        protected override void GenerateWarningMessages(IList<string> warnings)
        {
            base.GenerateWarningMessages(warnings);
            var binding = target as DataBinding;
            if (binding == null) return;

            if (binding.Target == null)
            {
                warnings.Add("Target not set.");
            }
        }
    }
}
