using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ComponentViewDrawer]
    internal class StringDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.DelayedTextField(name, (string)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(string);
        }
    }
}