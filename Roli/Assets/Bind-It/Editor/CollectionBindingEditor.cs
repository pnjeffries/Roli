using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Binding;

[CustomEditor(typeof(CollectionBinding))]
public class CollectionBindingEditor : BindingEditorBase
{
    private int _AssemblySelection = -1;

    private int _NamespaceSelection = -1;

    private Type _TypeSelection = null;

    //private bool _ParameterOf = false;

    private bool _GroupFoldout = false;

    private bool _TemplatesFoldout = true;

    protected override void BasicProperties()
    {
        base.BasicProperties();

        var binding = (CollectionBinding)target;

        //EditorGUILayout.Space();

        _GroupFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_GroupFoldout, "Grouping");
        if (_GroupFoldout)
        {
            AddPropertyField(nameof(CollectionBinding.Group));
            AddPropertyField(nameof(CollectionBinding.GroupPath));
            AddPropertyField(nameof(CollectionBinding.GroupTemplate));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //EditorGUILayout.Space();
        _TemplatesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_TemplatesFoldout, "Item Templates");
        if (_TemplatesFoldout)
        {

            // Item templates table
            for (int i = 0; i < binding.ItemTemplates.Count(); i++)
            {
                var template = binding.ItemTemplates[i];
                if (template != null)
                {
                    string typeName = template.DisplayName;
                    template.Template = EditorGUILayout.ObjectField(typeName, template.Template, typeof(GameObject), true) as GameObject;
                }
            }

            // Parameter Of
            //_ParameterOf = EditorGUILayout.Toggle("Parameter Of", _ParameterOf);

            // Assembly
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyNames = new string[assemblies.Length];
            for (int i = 0; i < assemblies.Length; i++)
            {
                assemblyNames[i] = assemblies[i].GetName().Name;
            }
            if (_TypeSelection != null)
            {
                _AssemblySelection = Array.IndexOf(assemblies, _TypeSelection.Assembly);
            }
            int prevAssemblySelection = _AssemblySelection;
            _AssemblySelection = EditorGUILayout.Popup("Assembly", _AssemblySelection, assemblyNames);
            if (_AssemblySelection != prevAssemblySelection)
            {
                _NamespaceSelection = -1;
                _TypeSelection = null;
            }

            if (_AssemblySelection >= 0 && _AssemblySelection < assemblies.Length)
            {
                Assembly assembly = assemblies[_AssemblySelection];

                // Namespace
                var namespaces = assembly.GetNamespaces().ToArray();
                if (_TypeSelection != null)
                {
                    _NamespaceSelection = Array.IndexOf(namespaces, _TypeSelection.Namespace);
                }
                int prevNamespaceSelection = _NamespaceSelection;
                _NamespaceSelection = EditorGUILayout.Popup("Namespace", _NamespaceSelection, namespaces);
                if (_NamespaceSelection != prevNamespaceSelection)
                {
                    _TypeSelection = null;
                }

                if (_NamespaceSelection >= 0 && _NamespaceSelection < namespaces.Length)
                {

                    string @namespace = namespaces[_NamespaceSelection];

                    // Type
                    Type[] types = assembly.GetTypesInNamespace(@namespace).ToArray();
                    int typeSelection = -1;
                    if (_TypeSelection != null) typeSelection = Array.IndexOf(types, _TypeSelection);
                    var typeNames = new string[types.Length];
                    for (int i = 0; i < types.Length; i++)
                    {
                        typeNames[i] = types[i].Name;
                    }
                    typeSelection = EditorGUILayout.Popup("Type", typeSelection, typeNames);
                    if (typeSelection >= 0 && typeSelection < types.Length)
                    {
                        _TypeSelection = types[typeSelection];
                        //template.TypeShortName = template.TargetType?.Name;

                        if (GUILayout.Button("Add"))
                        {
                            Type type = _TypeSelection;
                            /*if (type != null && _ParameterOf)
                            {
                                type = typeof(Parameter<>).MakeGenericType(_TypeSelection);
                            }*/
                            binding.AddItemTemplate(type);
                        }

                    }
                }
            }

            if (binding.ItemTemplates.Where(t => t.TargetType == null).Count() == 0)
            {
                if (GUILayout.Button("Add [Default]"))
                {
                    binding.AddItemTemplate(null);
                }
            }

            if (GUILayout.Button("Add Enums"))
            {
                /*if (_ParameterOf) binding.AddEnumsTemplate(typeof(Parameter));
                else*/
                binding.AddEnumsTemplate(null);
            }

            if (GUILayout.Button("Clear Unassigned"))
            {
                binding.ClearUnassignedItemTemplates();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (GUI.changed) EditorUtility.SetDirty(binding);
        //template.TypeShortName = EditorGUILayout.TextField("TypeName", template.TypeShortName);
    }
}
