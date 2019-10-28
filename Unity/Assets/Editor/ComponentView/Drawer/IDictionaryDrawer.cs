using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class IDictionaryDrawer : IComponentViewDrawer
    {
        public int Priority => DrawerPriority.Dictionary;

        private readonly List<object> keys = new List<object>();
        private readonly List<object> values = new List<object>();

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (IDictionary)value;
                Type[] elementTypes = GetElementTypes(v);
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;
                    int len = count / 10 + 1;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
                    EditorGUILayout.LabelField(count.ToString());
                    EditorGUILayout.EndHorizontal();

                    keys.Clear();
                    foreach (var item in v.Keys)
                    {
                        keys.Add(item);
                    }

                    values.Clear();
                    foreach (var item in v.Values)
                    {
                        values.Add(item);
                    }

                    for (int i = 0; i < keys.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ComponentViewHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));
                        string showName = $"{i}";
                        EditorGUILayout.LabelField(showName, GUILayout.Width(len * 5 + 15));

                        EditorGUILayout.BeginVertical();
                        keys[i] = ComponentViewHelper.DrawAndGetNewValue(elementTypes[0], keys[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            ShowName = "Key:",
                            ShowNameWidth = 30,
                            IsStatic = false,
                            FieldName = field.Name + $"_{i}_Key"
                        }, field);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        values[i] = ComponentViewHelper.DrawAndGetNewValue(elementTypes[1], values[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            ShowName = "Value:",
                            ShowNameWidth = 40,
                            IsStatic = false,
                            FieldName = field.Name + $"_{i}_Value"
                        }, field);
                        EditorGUILayout.EndVertical();

                        if (!v.IsFixedSize)
                        {
                            if (GUILayout.Button("x", GUILayout.Width(20)))
                            {
                                keys.RemoveAt(i);
                                values.RemoveAt(i);
                                EditorGUILayout.EndHorizontal();
                                break;
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    v.Clear();
                    for (int i = 0; i < keys.Count; i++)
                    {
                        v.Add(keys[i], values[i]);
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
                }

                value = v;
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        private Type[] GetElementTypes(IDictionary v)
        {
            Type[] ts = new Type[2];
            try
            {
                Type type = v.GetType();
                if (type.IsGenericType)
                {
                    Type[] types = type.GetGenericArguments();
                    if (types.Length != 2)
                    {
                        Debug.Log($"{type}参数不是2个:{string.Join<Type>(";", types)}");
                    }
                    else
                    {
                        ts[0] = types[0];
                        ts[1] = types[1];
                    }
                }
                else
                {
                    Debug.Log($"{type}不是泛型类型");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return ts;
        }

        private void AddOne(ref IDictionary v)
        {
            try
            {
                Type type = v.GetType();
                if (type.IsGenericType)
                {
                    Type[] types = type.GetGenericArguments();
                    if (types.Length != 2)
                    {
                        Debug.Log($"{type}参数不是2个:{string.Join<Type>(";", types)}");
                    }
                    else
                    {
                        v.Add(ComponentViewHelper.CreateInstance(types[0]), ComponentViewHelper.CreateInstance(types[1]));
                    }
                }
                else
                {
                    Debug.Log($"{type}不是泛型类型");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public bool TypeEquals(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }
    }
}