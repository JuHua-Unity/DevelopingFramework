using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class StringDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.String;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedTextField(draw.ShowName, (string)value);
                }
                else
                {
                    value = EditorGUILayout.TextField(draw.ShowName, (string)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedTextField((string)value);
                }
                else
                {
                    value = EditorGUILayout.TextField((string)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedTextField((string)value);
                }
                else
                {
                    value = EditorGUILayout.TextField((string)value);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(string);
        }
    }
}