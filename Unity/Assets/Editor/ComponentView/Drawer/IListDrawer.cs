using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class IListDrawer : IComponentViewDrawer
    {
        public int Priority => DrawerPriority.List;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.ShowName);
            }
            else
            {
                var v = (IList)value;
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;

                    EditorGUI.BeginDisabledGroup(v.IsReadOnly);

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
                            AddOne(ref v);
                        }

                        for (int i = count; i < count_T; i++)
                        {
                            v.RemoveAt(count);
                        }
                    }

                    for (int i = 0; i < v.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ComponentViewHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));

                        EditorGUILayout.BeginVertical();
                        string showName = $"{i}";
                        if (v[i] == null)
                        {
                            ComponentViewHelper.ShowNull(showName);
                        }
                        else
                        {
                            v[i] = ComponentViewHelper.DrawAndGetNewValue(v[i], new DrawInfo()
                            {
                                Changeable = true,
                                ShowName = showName,
                                ShowNameWidth = len * 5 + 15,
                                IsStatic = false,
                                FieldName = field.Name + $"_{i}"
                            }, field);
                        }
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
                            AddOne(ref v);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.EndDisabledGroup();
                }

                value = v;
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        private void AddOne(ref IList v)
        {
            try
            {
                Type type = v.GetType();
                if (type.IsGenericType)
                {
                    Type[] types = type.GetGenericArguments();
                    if (types.Length < 1)
                    {
                        Debug.Log("泛型类型，但没有参数");
                    }
                    else if (types.Length == 1)
                    {
                        v.Add(GetNewElementInstance(types[0]));
                    }
                    else
                    {
                        v.Add(GetNewElementInstance(types[types.Length - 1]));
                    }
                }
                else
                {
                    Debug.Log("不是泛型类型");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static object GetNewElementInstance(Type type)
        {
            object o = null;
            if (type == typeof(string))
            {
                o = string.Empty;
            }
            else
            {
                o = Activator.CreateInstance(type);
            }

            return o;
        }

        public bool TypeEquals(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }
    }
}