using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ObjectDrawer]
    internal class ObjectDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Object;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
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