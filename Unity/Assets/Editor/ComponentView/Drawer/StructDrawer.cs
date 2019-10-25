using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class StructDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
            fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
            ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
            if (fold)
            {
                EditorGUILayout.BeginHorizontal();
                ComponentViewHelper.Tab();
                ComponentViewHelper.Draw(value);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type.IsValueType && !type.IsEnum && !type.IsPrimitive;
        }
    }
}