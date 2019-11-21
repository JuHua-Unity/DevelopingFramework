using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class KeyValuePairDrawer : IObjectDrawer
    {
        private static readonly Type genericType = typeof(KeyValuePair<,>);

        private Type type;
        private readonly List<object> list = new List<object>();

        public int Priority => DrawerPriority.KeyValuePair;

        public void Draw(object value, FieldInfo field = null)
        {
            type = value.GetType();
            EditorGUILayout.BeginHorizontal();
            if (field != null)
            {
                EditorGUILayout.LabelField(field.Name);
            }

            EditorGUILayout.LabelField("Key:", GUILayout.Width(50));
            ObjectDrawerHelper.Draw(type.GetProperty("Key").GetValue(value), null);
            EditorGUILayout.LabelField("Value:", GUILayout.Width(60));
            ObjectDrawerHelper.Draw(type.GetProperty("Value").GetValue(value), null);
            EditorGUILayout.EndHorizontal();
        }

        public bool TypeEquals(Type type)
        {
            if (type.IsGenericType)
            {
                var ts = type.GetGenericArguments();
                if (ts.Length == 2)
                {
                    if (genericType.MakeGenericType(ts[0], ts[1]) == type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}