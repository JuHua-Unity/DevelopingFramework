using System;
using System.Reflection;
using UnityEditor;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class ValueTypeDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.LabelField(value.ToString());
            }
            else
            {
                EditorGUILayout.LabelField(field.Name, value.ToString());
            }
        }

        public bool TypeEquals(Type t)
        {
            return t.IsValueType && t.IsPrimitive;
        }
    }
}