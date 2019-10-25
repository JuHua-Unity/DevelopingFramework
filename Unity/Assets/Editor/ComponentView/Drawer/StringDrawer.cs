using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class StringDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.DelayedTextField(draw.FieldName, (string)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.DelayedTextField((string)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.DelayedTextField((string)value);

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