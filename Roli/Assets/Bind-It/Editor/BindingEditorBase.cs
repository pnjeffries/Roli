using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// Abstract base class for binding editors 
    /// </summary>
    public abstract class BindingEditorBase : Editor
    {
        protected void AddPropertyField(string name)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(name));
        }

        protected virtual void BasicProperties()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DataBinding.Source)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DataBinding.Path)));
        }

        protected virtual void GenerateWarningMessages(IList<string> warnings)
        {
            var binding = target as SingleBindingBase;
            if (binding.Source == null && binding.GetComponentInParent<DataContext>() == null)
            {
                warnings.Add(
                    "Source is null and no DataContext can be found in the object hierarchy to inherit a source object from.");
            }
        }

        protected IList<string> GenerateWarningMessages()
        {
            var warnings = new List<string>();
            GenerateWarningMessages(warnings);
            return warnings;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BasicProperties();

            // Warnings:
            var warnings = GenerateWarningMessages();
            if (warnings.Count > 0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < warnings.Count; i++)
                {
                    sb.AppendLine(warnings[i]);
                }
                EditorGUILayout.HelpBox(sb.ToString(), MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
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
