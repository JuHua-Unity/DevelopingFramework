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

            FieldInfo[] filedInfos = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (filedInfos.Length > 0)
            {
                EditorGUILayout.BeginVertical();

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

            //属性会带这个字符
            name = name.Replace("k__BackingField", "");

            for (int i = 0; i < drawers.Count; i++)
            {
                if (drawers[i].TypeEquals(type))
                {
                    value = drawers[i].DrawAndGetNewValue(type, value, new DrawInfo() { Changeable = changeable, ShowName = name, ShowNameWidth = -1, IsStatic = staticField, FieldName = field.Name }, field);
                    return value;
                }
            }

            ShowUnrecognized(name);
            return value;
        }

        public static object DrawAndGetNewValue(object value, DrawInfo draw, FieldInfo field)
        {
            Type type = value.GetType();

            for (int i = 0; i < drawers.Count; i++)
            {
                if (drawers[i].TypeEquals(type))
                {
                    value = drawers[i].DrawAndGetNewValue(type, value, draw, field);
                    return value;
                }
            }

            ShowUnrecognized(draw.ShowName);
            return value;
        }

        public static void ShowNull(string name)
        {
            EditorGUILayout.LabelField(name, "null");
        }

        public static void Tab()
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20));
        }

        public static void ShowUnrecognized(string name)
        {
            EditorGUILayout.LabelField(name, "unrecognized");
        }

        #region FieldShow

        private static readonly Dictionary<string, FieldShow> fieldShows = new Dictionary<string, FieldShow>();

        public static bool GetAndAddFieldShow_Fold(string fieldName)
        {
            if (!fieldShows.ContainsKey(fieldName))
            {
                fieldShows.Add(fieldName, new FieldShow() { Fold = false });
            }

            return fieldShows[fieldName].Fold;
        }

        public static void SetAndAddFieldShow_Fold(string fieldName, bool fold)
        {
            if (!fieldShows.ContainsKey(fieldName))
            {
                fieldShows.Add(fieldName, new FieldShow() { Fold = false });
            }

            fieldShows[fieldName] = new FieldShow() { Fold = fold };
        }

        private struct FieldShow
        {
            public bool Fold;
        }

        #endregion
    }

    /// <summary>
    /// 显示的一些定义
    /// </summary>
    public struct DrawInfo
    {
        /// <summary>
        /// 字段显示名字
        /// </summary>
        public string ShowName;

        /// <summary>
        /// 字段名字
        /// </summary>
        public string FieldName;

        /// <summary>
        /// 字段显示的宽度
        /// -1 系统默认
        /// 0 不显示
        /// >0 按宽度显示
        /// </summary>
        public int ShowNameWidth;

        /// <summary>
        /// 是否可以修改
        /// </summary>
        public bool Changeable;

        /// <summary>
        /// 是否是静态变量
        /// </summary>
        public bool IsStatic;
    }
}