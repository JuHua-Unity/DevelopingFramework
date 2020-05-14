using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class KeyValuePairDrawer : IObjectDrawer
    {
        private static readonly Type genericType = typeof(KeyValuePair<,>);
        private Type type;

        public int Priority => DrawerPriority.KeyValuePair;

        public void Draw(object value, FieldInfo field = null)
        {
            this.type = value.GetType();
            if (field != null)
            {
                EditorGUILayout.LabelField(field.Name);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key:", GUILayout.Width(50));
            ObjectDrawerHelper.Draw(this.type.GetProperty("Key")?.GetValue(value));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value:", GUILayout.Width(60));
            ObjectDrawerHelper.Draw(this.type.GetProperty("Value")?.GetValue(value));
            EditorGUILayout.EndHorizontal();
        }

        public bool TypeEquals(Type t)
        {
            if (t.IsGenericType)
            {
                var ts = t.GetGenericArguments();
                if (ts.Length == 2)
                {
                    if (genericType.MakeGenericType(ts[0], ts[1]) == t)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}