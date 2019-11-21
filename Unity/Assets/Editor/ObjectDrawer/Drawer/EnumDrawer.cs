using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ObjectDrawer]
    internal class EnumDrawer : IObjectDrawer
    {
        private Type type;
        private Enum v;

        public int Priority => DrawerPriority.Enum;

        public void Draw(object value, FieldInfo field = null)
        {
            type = value.GetType();
            v = (Enum)value;
            if (field == null)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    EditorGUILayout.EnumFlagsField(v);
                }
                else
                {
                    EditorGUILayout.EnumPopup(v);
                }
            }
            else
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    EditorGUILayout.EnumFlagsField(field.Name, v);
                }
                else
                {
                    EditorGUILayout.EnumPopup(field.Name, v);
                }
            }
        }

        public bool TypeEquals(Type type)
        {
            return type.IsEnum;
        }
    }
}