﻿using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class StackDrawer : IComponentViewDrawer
    {
        public int Priority => DrawerPriority.Stack;

        private int selectIndex = 0;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ComponentViewHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (Stack)value;
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

                    var array = v.ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ComponentViewHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));

                        EditorGUILayout.BeginVertical();
                        string showName = $"{i}";
                        array[i] = ComponentViewHelper.DrawAndGetNewValue(array[i].GetType(), array[i], new DrawInfo()
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

                    v.Clear();
                    for (int i = array.Length - 1; i >= 0; i--)
                    {
                        v.Push(array[i]);
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("");
                    if (v.Count > 0)
                    {
                        if (GUILayout.Button("Pop", GUILayout.Width(40)))
                        {
                            v.Pop();
                        }
                    }
                    if (GUILayout.Button("Push", GUILayout.Width(40)))
                    {
                        v.Push(ComponentViewHelper.CreateInstance(selectIndex));
                    }
                    selectIndex = EditorGUILayout.Popup(selectIndex, ComponentViewHelper.SelectTypeNames, GUILayout.Width(80));
                    EditorGUILayout.EndHorizontal();
                }

                value = v;
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Stack);
        }
    }
}