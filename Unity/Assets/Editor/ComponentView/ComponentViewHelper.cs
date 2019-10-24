using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    internal static class ComponentViewHelper
    {
        private static readonly List<IComponentViewDrawer> drawers = null;

        static ComponentViewHelper()
        {
            drawers = new List<IComponentViewDrawer>();
            Assembly assembly = typeof(ComponentViewHelper).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsDefined(typeof(ComponentViewDrawerAttribute)))
                {
                    continue;
                }

                IComponentViewDrawer drawer = (IComponentViewDrawer)Activator.CreateInstance(type);
                drawers.Add(drawer);
            }
        }

        public static void Draw(object obj)
        {
            if (drawers == null || drawers.Count < 1)
            {
                return;
            }

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
                value = DrawAndGetNewValue(type, field, value);
                field.SetValue(obj, value);
            }

            EditorGUILayout.EndVertical();
        }

        private static object DrawAndGetNewValue(Type type, FieldInfo field, object value)
        {
            bool changeable = field.IsPublic;
            bool staticField = field.IsStatic;
            string name = field.Name;
            if (staticField)
            {
                name = $"Static:{name}";
            }

            for (int i = 0; i < drawers.Count; i++)
            {
                if (drawers[i].TypeEquals(type))
                {
                    value = drawers[i].DrawAndGetNewValue(type, name, value, changeable, staticField, field);
                    return value;
                }
            }

            return value;
        }
    }
}