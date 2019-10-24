using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ComponentViewDrawer]
    internal class EnumDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            if (type.IsDefined(typeof(FlagsAttribute), false))
            {
                value = EditorGUILayout.EnumFlagsField(name, (Enum)value);
            }
            else
            {
                value = EditorGUILayout.EnumPopup(name, (Enum)value);
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