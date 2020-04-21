using System;
using System.Reflection;
using UnityEditor;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class EnumDrawer : IObjectDrawer
    {
        private Type type;
        private Enum v;

        public int Priority => DrawerPriority.Enum;

        public void Draw(object value, FieldInfo field = null)
        {
            this.type = value.GetType();
            this.v = (Enum) value;
            if (field == null)
            {
                if (this.type.IsDefined(typeof(FlagsAttribute), false))
                {
                    EditorGUILayout.EnumFlagsField(this.v);
                }
                else
                {
                    EditorGUILayout.EnumPopup(this.v);
                }
            }
            else
            {
                if (this.type.IsDefined(typeof(FlagsAttribute), false))
                {
                    EditorGUILayout.EnumFlagsField(field.Name, this.v);
                }
                else
                {
                    EditorGUILayout.EnumPopup(field.Name, this.v);
                }
            }
        }

        public bool TypeEquals(Type t)
        {
            return t.IsEnum;
        }
    }
}