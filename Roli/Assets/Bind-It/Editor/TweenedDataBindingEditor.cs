using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Binding
{
    [CustomEditor(typeof(TweenedDataBinding), true)]
    public class TweenedDataBindingEditor : DataBindingEditor
    {
        bool _ShowFoldout = true;

        protected override void BasicProperties()
        {
            base.BasicProperties();

            _ShowFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_ShowFoldout, "Tweening");
            if (_ShowFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TweenedDataBinding.Curve)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TweenedDataBinding.Duration)));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

}