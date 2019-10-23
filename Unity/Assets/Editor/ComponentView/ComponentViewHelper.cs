#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    internal static class ComponentViewHelper
    {
        public static void Draw(object obj)
        {
            EditorGUILayout.BeginVertical();

            FieldInfo[] filedInfos = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in filedInfos)
            {
                Type type = field.FieldType;
                if (type.IsDefined(typeof(HideInInspector), false))
                {
                    continue;
                }

                if (field.IsDefined(typeof(HideInInspector), false))
                {
                    continue;
                }

                object value = field.GetValue(obj);
                value = GetNewValue(type, field, value);
                field.SetValue(obj, value);
            }

            EditorGUILayout.EndVertical();
        }

        private static object GetNewValue(Type type, FieldInfo field, object value)
        {
            bool changeable = field.IsPublic;
            bool staticField = field.IsStatic;
            string name = field.Name;
            if (staticField)
            {
                name = $"Static:{name}";
            }

            EditorGUI.BeginDisabledGroup(!changeable);
            if (type == typeof(int))
            {
                var v = (int)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                int.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(float))
            {
                var v = (float)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                float.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(bool))
            {
                value = EditorGUILayout.Toggle(name, (bool)value);
            }

            if (type == typeof(long))
            {
                var v = (long)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                long.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(double))
            {
                var v = (double)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                double.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(byte))
            {
                var v = (byte)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                byte.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(char))
            {
                var v = (char)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                char.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(decimal))
            {
                var v = (decimal)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                decimal.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(sbyte))
            {
                var v = (sbyte)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                sbyte.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(short))
            {
                var v = (short)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                short.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(uint))
            {
                var v = (uint)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                uint.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(ulong))
            {
                var v = (ulong)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                ulong.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(ushort))
            {
                var v = (ushort)value;
                string str = EditorGUILayout.TextField(name, v.ToString());
                ushort.TryParse(str, out v);
                value = v;
            }

            if (type == typeof(string))
            {
                value = EditorGUILayout.TextField(name, (string)value);
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }
    }
}
#endif