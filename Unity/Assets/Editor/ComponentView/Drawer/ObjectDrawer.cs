using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ComponentViewDrawer]
    internal class ObjectDrawer : IComponentViewDrawer
    {
        public int Priority => DrawerPriority.Object;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.ShowName);
            }
            else
            {
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    EditorGUILayout.BeginHorizontal();
                    ComponentViewHelper.Tab();
                    ComponentViewHelper.DrawObj(value);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type != typeof(string) && !type.IsValueType;
        }
    }
}