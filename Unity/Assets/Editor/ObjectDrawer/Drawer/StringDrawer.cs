using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ObjectDrawer]
    internal class StringDrawer : IObjectDrawer
    {
        private Type type;
        private string name;

        public int Priority => DrawerPriority.String;

        public void Draw(object value, FieldInfo field = null)
        {
            type = value.GetType();
            if (field == null)
            {
                name = type.Name;
            }
            else
            {
                name = field.Name;
            }

            if (field == null)
            {
                EditorGUILayout.LabelField((string)value);
            }
            else
            {
                EditorGUILayout.LabelField(field.Name, (string)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(string);
        }
    }
}