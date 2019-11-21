using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class DelegateDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Delegate;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ObjectDrawerHelper.ShowNull(draw.ShowName, null, ref value);
            }
            else
            {
                var v = (Delegate)value;
                bool fold = ObjectDrawerHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ObjectDrawerHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    Delegate[] ds = v.GetInvocationList();
                    int len = ds.Length / 10 + 1;
                    for (int i = 0; i < ds.Length; i++)
                    {
                        var d = ds[i];
                        EditorGUILayout.BeginHorizontal();
                        ObjectDrawerHelper.Tab();
                        EditorGUILayout.LabelField("Delegate:", GUILayout.Width(60));
                        EditorGUILayout.LabelField($"{i}", GUILayout.Width(len * 5 + 15));
                        EditorGUILayout.LabelField($"{d.Method.ReflectedType}.{d.Method.Name}");
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("测试运行(如果该委托无参的话)"))
                    {
                        v.DynamicInvoke();
                    }
                }

                value = v;
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            if (type == typeof(Delegate))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(Delegate)))
            {
                return true;
            }

            return typeof(Delegate).IsAssignableFrom(type);
        }
    }
}