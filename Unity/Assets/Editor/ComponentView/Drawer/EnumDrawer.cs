using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class EnumDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    value = EditorGUILayout.EnumFlagsField(draw.FieldName, (Enum)value);
                }
                else
                {
                    value = EditorGUILayout.EnumPopup(draw.FieldName, (Enum)value);
                }
            }
            else if (draw.FieldNameWidth == 0)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    value = EditorGUILayout.EnumFlagsField((Enum)value);
                }
                else
                {
                    value = EditorGUILayout.EnumPopup((Enum)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    value = EditorGUILayout.EnumFlagsField((Enum)value);
                }
                else
                {
                    value = EditorGUILayout.EnumPopup((Enum)value);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type.IsEnum;
        }
    }
}