using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class ArrayDrawer : IComponentViewDrawer//此Array不是System.Array   指的是[]  (int[] string[]...)
    {
        public int Priority => DrawerPriority.Array;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (IList)value;
                Type elementType = GetElementType(v);
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;
                    int len = count / 10 + 1;
                    int count_T = v.Count;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
                    EditorGUILayout.LabelField(count.ToString());
                    EditorGUILayout.EndHorizontal();

                    for (int i = 0; i < v.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ComponentViewHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));

                        EditorGUILayout.BeginVertical();
                        string showName = $"{i}";
                        v[i] = ComponentViewHelper.DrawAndGetNewValue(elementType, v[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            ShowName = showName,
                            ShowNameWidth = len * 5 + 15,
                            IsStatic = false,
                            FieldName = field.Name + $"_{i}"
                        }, field);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.EndHorizontal();
                    }
                }

                value = v;
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        private Type GetElementType(IList v)
        {
            Type t = null;
            try
            {
                Type type = v.GetType();
                string name = type.FullName;
                if (name.EndsWith("[]"))
                {
                    string typeName = name.Substring(0, name.Length - 2);
                    foreach (Assembly b in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Type bT = b.GetType(typeName);
                        if (bT != null)
                        {
                            t = bT;
                            break;
                        }
                    }

                    if (t == null)
                    {
                        Debug.Log($"{type}的数组类型找不到");
                    }
                }
                else
                {
                    Debug.Log($"{type}不是数组");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return t;
        }

        public bool TypeEquals(Type type)
        {
            return typeof(Array).IsAssignableFrom(type);
        }
    }
}