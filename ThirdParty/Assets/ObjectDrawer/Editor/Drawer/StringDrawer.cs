using System;
using System.Reflection;
using UnityEditor;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class StringDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.String;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.LabelField((string) value);
            }
            else
            {
                EditorGUILayout.LabelField(field.Name, (string) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(string);
        }
    }
}