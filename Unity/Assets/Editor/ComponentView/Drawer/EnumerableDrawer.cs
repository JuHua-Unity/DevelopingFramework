using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class EnumerableDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.FieldName);
            }
            else
            {
                var v = (IList)value;
                bool fold = ComponentViewHelper.GetAndAddFieldShow_Fold(field);
                fold = EditorGUILayout.Foldout(fold, draw.FieldName);
                ComponentViewHelper.SetAndAddFieldShow_Fold(field, fold);
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
                            if (count_T > 0)
                            {
                                Debug.Log($"Add:{v[count_T - 1]}");
                                v.Add(v[count_T - 1]);
                            }
                            else
                            {
                                try
                                {
                                    Type t = (v.GetType().GetMember("Item")[0] as PropertyInfo).PropertyType;
                                    object o = t.IsValueType ? Activator.CreateInstance(t) : null;
                                    Debug.Log($"Add:{o}");
                                    v.Add(o);
                                }
                                catch (Exception e)
                                {
                                    Debug.LogException(e);
                                }
                            }
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
                        v[i] = ComponentViewHelper.DrawAndGetNewValue(v[i], new DrawInfo() { Changeable = true, FieldName = $"{i}", FieldNameWidth = len * 10, IsStatic = false }, field);
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
                            if (v.Count > 0)
                            {
                                v.Add(v[v.Count - 1]);
                            }
                            else
                            {
                                v.Add(v.GetEnumerator().Current);
                            }
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

        public bool TypeEquals(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }
    }
}