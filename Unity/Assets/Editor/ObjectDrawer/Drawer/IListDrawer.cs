using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class IListDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.List;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (IList)value;
                Type elementType = GetElementType(v);
                bool fold = ObjectDrawerHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ObjectDrawerHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;
                    int len = count / 10 + 1;
                    int count_T = v.Count;
                    if (v.IsFixedSize)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
                        EditorGUILayout.LabelField(count.ToString());
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
                        count = EditorGUILayout.DelayedIntField(count);
                        count = count < 0 ? 0 : count;
                        EditorGUILayout.EndHorizontal();

                        for (int i = v.Count; i < count; i++)
                        {
                            AddOne(elementType, ref v);
                        }

                        for (int i = count; i < count_T; i++)
                        {
                            v.RemoveAt(count);
                        }
                    }

                    for (int i = 0; i < v.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ObjectDrawerHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));

                        EditorGUILayout.BeginVertical();
                        string showName = $"{i}";
                        v[i] = ObjectDrawerHelper.DrawAndGetNewValue(elementType, v[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            NeedDelayed = draw.NeedDelayed,
                            ShowName = showName,
                            ShowNameWidth = len * 5 + 15,
                            IsStatic = false,
                            FieldName = field.Name + $"_{i}"
                        }, field);
                        EditorGUILayout.EndVertical();

                        if (!v.IsFixedSize)
                        {
                            if (GUILayout.Button("x", GUILayout.Width(20)))
                            {
                                v.RemoveAt(i);
                                EditorGUILayout.EndHorizontal();
                                break;
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (!v.IsFixedSize)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("");
                        if (GUILayout.Button("+", GUILayout.Width(20)))
                        {
                            AddOne(elementType, ref v);
                        }
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
                if (type.IsGenericType)
                {
                    Type[] types = type.GetGenericArguments();
                    if (types.Length < 1)
                    {
                        Debug.Log($"{type}是泛型类型，但没有参数");
                    }
                    else if (types.Length == 1)
                    {
                        t = types[0];
                    }
                    else
                    {
                        Debug.Log($"{type}是泛型类型，但有{types.Length}个参数：{string.Join<Type>(";", types)}");
                        t = types[types.Length - 1];
                    }
                }
                else
                {
                    string name = type.Name;
                    if (name.EndsWith("[]"))
                    {
                        t = Type.GetType(name.Substring(0, name.Length - 2));
                    }
                    else
                    {
                        Debug.Log($"{type}不是泛型类型，但却不是数组");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return t;
        }

        private void AddOne(Type type, ref IList v)
        {
            try
            {
                if (type != null)
                {
                    v.Add(ObjectDrawerHelper.CreateInstance(type));
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type.IsGenericType && typeof(IList).IsAssignableFrom(type);
        }
    }
}