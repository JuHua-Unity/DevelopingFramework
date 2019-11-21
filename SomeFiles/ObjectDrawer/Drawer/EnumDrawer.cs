using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class EnumDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Enum;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    value = EditorGUILayout.EnumFlagsField(draw.ShowName, (Enum)value);
                }
                else
                {
                    value = EditorGUILayout.EnumPopup(draw.ShowName, (Enum)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
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

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
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