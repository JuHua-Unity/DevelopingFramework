﻿using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class ICollectionDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Collection;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (value == null)
            {
                ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
            }
            else
            {
                var v = (ICollection)value;
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

                    object[] array = new object[v.Count];
                    v.CopyTo(array, 0);
                    for (int i = 0; i < array.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ObjectDrawerHelper.Tab();
                        EditorGUILayout.LabelField("Element:", GUILayout.Width(60));

                        EditorGUILayout.BeginVertical();
                        string showName = $"{i}";
                        array[i] = ObjectDrawerHelper.DrawAndGetNewValue(array[i].GetType(), array[i], new DrawInfo()
                        {
                            Changeable = draw.Changeable,
                            NeedDelayed = draw.NeedDelayed,
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

        public bool TypeEquals(Type type)
        {
            return type.IsGenericType && typeof(ICollection).IsAssignableFrom(type) && !typeof(IList).IsAssignableFrom(type) && !typeof(IDictionary).IsAssignableFrom(type);
        }
    }
}