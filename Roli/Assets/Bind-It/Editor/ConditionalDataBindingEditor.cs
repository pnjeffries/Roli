using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Binding
{
    [CustomEditor(typeof(ConditionalDataBinding))]
    public class ConditionalDataBindingEditor : DataBindingEditor
    {
        protected override void BasicProperties()
        {

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ConditionalDataBinding.Comparison)));
            //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ConditionalDataBinding.CompareTo)));

            base.BasicProperties();

        }
    }
}
