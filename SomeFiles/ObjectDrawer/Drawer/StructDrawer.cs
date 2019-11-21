using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class StructDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Struct;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            bool fold = ObjectDrawerHelper.GetAndAddFieldShow_Fold(draw.FieldName);
            fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
            ObjectDrawerHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
            if (fold)
            {
                EditorGUILayout.BeginHorizontal();
                ObjectDrawerHelper.Tab();
                ObjectDrawerHelper.DrawObj(value, draw.NeedDelayed);
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