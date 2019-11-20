using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class IDictionaryDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Dictionary;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (IDictionary)value;
                bool fold = ObjectDrawerHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ObjectDrawerHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;
                    int len = count / 10 + 1;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
                    EditorGUILayout.LabelField(count.ToString());
                    EditorGUILayout.EndHorizontal();

                    List<object> keys = new List<object>();
                    List<object> values = new List<object>();

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
                        ObjectDrawerHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));
                        string showName = $"{i}";
                        EditorGUILayout.LabelField(showName, GUILayout.Width(len * 5 + 15));

                        EditorGUILayout.BeginVertical();
                        keys[i] = ObjectDrawerHelper.DrawAndGetNewValue(keys[i].GetType(), keys[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            NeedDelayed = draw.NeedDelayed,
                            ShowName = "Key:",
                            ShowNameWidth = 30,
                            IsStatic = false,
                            FieldName = field.Name + $"_{i}_Key"
                        }, field);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        values[i] = ObjectDrawerHelper.DrawAndGetNewValue(values[i].GetType(), values[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            NeedDelayed = draw.NeedDelayed,
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
                        v.Add(ObjectDrawerHelper.CreateInstance(types[0]), ObjectDrawerHelper.CreateInstance(types[1]));
                    }
                }
                else
                {
                    Debug.Log($"{type}没有明确的类型表示");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type);
        }
    }
}