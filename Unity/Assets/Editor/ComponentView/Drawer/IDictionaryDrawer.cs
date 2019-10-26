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
                ComponentViewHelper.ShowNull(draw.ShowName);
            }
            else
            {
                var v = (IDictionary)value;
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(draw.FieldName);
                fold = EditorGUILayout.Foldout(fold, draw.ShowName, true);
                ComponentViewHelper.SetAndAddFieldShow_Fold(draw.FieldName, fold);
                if (fold)
                {
                    int count = v.Count;

                    EditorGUI.BeginDisabledGroup(v.IsReadOnly);

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
                        if (keys[i] == null)
                        {
                            ComponentViewHelper.ShowNull("Key:");
                        }
                        else
                        {
                            keys[i] = ComponentViewHelper.DrawAndGetNewValue(keys[i], new DrawInfo()
                            {
                                Changeable = true,
                                ShowName = "Key:",
                                ShowNameWidth = 30,
                                IsStatic = false,
                                FieldName = field.Name + $"_{i}_Key"
                            }, field);
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        if (values[i] == null)
                        {
                            ComponentViewHelper.ShowNull("Value:");
                        }
                        else
                        {
                            values[i] = ComponentViewHelper.DrawAndGetNewValue(values[i], new DrawInfo()
                            {
                                Changeable = true,
                                ShowName = "Value:",
                                ShowNameWidth = 40,
                                IsStatic = false,
                                FieldName = field.Name + $"_{i}_Value"
                            }, field);
                        }
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

                    EditorGUI.EndDisabledGroup();
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
                var types = v.GetType().GetGenericArguments();
                if (types != null)
                {
                    if (types.Length == 2)
                    {
                        object o1 = null;
                        if (types[0] == typeof(string))
                        {
                            o1 = string.Empty;
                        }
                        else
                        {
                            o1 = Activator.CreateInstance(types[0]);
                        }

                        object o2 = null;
                        if (types[1] == typeof(string))
                        {
                            o2 = string.Empty;
                        }
                        else
                        {
                            o2 = Activator.CreateInstance(types[1]);
                        }

                        v.Add(o1, o2);
                    }
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