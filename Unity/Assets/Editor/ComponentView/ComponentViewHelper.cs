using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    internal static class ComponentViewHelper
    {
        private static readonly BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private static readonly List<IComponentViewDrawer> drawers = null;

        private static readonly List<Type> selectTypes = null;
        public static readonly string[] SelectTypeNames = null;

        static ComponentViewHelper()
        {
            List<IComponentViewDrawer> list = new List<IComponentViewDrawer>();
            Assembly assembly = typeof(ComponentViewHelper).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsDefined(typeof(ComponentViewDrawerAttribute)))
                {
                    continue;
                }

                IComponentViewDrawer drawer = (IComponentViewDrawer)Activator.CreateInstance(type);
                list.Add(drawer);
            }

            if (list.Count > 2)
            {
                drawers = new List<IComponentViewDrawer>() { list[0] };
                for (int i = 1; i < list.Count; i++)
                {
                    var p = list[i].Priority;
                    bool inserted = false;
                    for (int j = 0; j < drawers.Count; j++)
                    {
                        if (p <= drawers[j].Priority)
                        {
                            drawers.Insert(j, list[i]);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        drawers.Add(list[i]);
                    }
                }
            }
            else
            {
                drawers = new List<IComponentViewDrawer>(list);
            }

            var types = CollectTypes();
            selectTypes = new List<Type>
            {
                typeof(string),
                typeof(int),
                typeof(float),
                typeof(bool),
                typeof(long),
                typeof(double),
                typeof(byte),
                typeof(char),
                typeof(decimal),
                typeof(sbyte),
                typeof(short),
                typeof(uint),
                typeof(ulong),
                typeof(ushort)
            };
            for (int i = 0; i < types.Length; i++)
            {
                if (!selectTypes.Contains(types[i]))
                {
                    selectTypes.Add(types[i]);
                }
            }
            SelectTypeNames = new string[selectTypes.Count];
            for (int i = 0; i < selectTypes.Count; i++)
            {
                SelectTypeNames[i] = selectTypes[i].Name;
            }
        }

        public static void Draw(object obj)
        {
            Switch();

            if (!show)
            {
                return;
            }

            if (drawers == null || drawers.Count < 1)
            {
                return;
            }

            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);
            List<FieldInfo> fs = new List<FieldInfo>();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var f = fieldInfos[i];
                if (f.FieldType != type && !type.IsSubclassOf(f.FieldType) && !f.FieldType.IsSubclassOf(type))
                {
                    fs.Add(f);
                }
            }

            fieldInfos = Filter(fs.ToArray());

            EditorGUI.BeginDisabledGroup(!enable);

            if (fieldInfos.Length > 0)
            {
                EditorGUILayout.BeginVertical();
                DrawObj(obj, fieldInfos);
                EditorGUILayout.EndVertical();
            }

            EditorGUI.EndDisabledGroup();
        }

        #region Filter

        private static string filterStr = string.Empty;
        private static readonly List<FieldInfo> filterFieldInfos = new List<FieldInfo>();

        private static FieldInfo[] Filter(FieldInfo[] fieldInfos)
        {
            filterFieldInfos.Clear();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("搜索：", GUILayout.Width(100));
            filterStr = EditorGUILayout.TextField(filterStr).ToLower();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                string name = fieldInfos[i].Name;
                if (fieldInfos[i].IsStatic)
                {
                    name = $"Static:{name}";
                }
                name = name.ToLower();
                if (name.Contains(filterStr))
                {
                    filterFieldInfos.Add(fieldInfos[i]);
                }
            }
            return filterFieldInfos.ToArray();
        }

        #endregion

        #region Switch

        private static bool show = true;
        private static bool enable = false;

        private static void Switch()
        {
            EditorGUILayout.BeginHorizontal();

            show = EditorGUILayout.ToggleLeft("Visible", show);
            enable = EditorGUILayout.ToggleLeft("Enable", enable);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        #endregion

        public static void DrawObj(object obj, FieldInfo[] fieldInfos = null)
        {
            if (fieldInfos == null)
            {
                fieldInfos = obj.GetType().GetFields(bindingFlags);
            }

            EditorGUILayout.BeginVertical();

            foreach (FieldInfo field in fieldInfos)
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
            bool changeable = true;
            bool staticField = field.IsStatic;
            string name = field.Name;

            //属性会带这个字符
            if (name.Contains("k__BackingField"))
            {
                changeable = false;
                name = "P:" + name.Replace("k__BackingField", "").Trim().TrimStart('<').TrimEnd('>').Trim();
            }

            if (staticField)
            {
                name = $"Static:{name}";
            }

            var draw = new DrawInfo()
            {
                Changeable = changeable,
                ShowName = name,
                ShowNameWidth = -1,
                IsStatic = staticField,
                FieldName = field.Name
            };
            for (int i = 0; i < drawers.Count; i++)
            {
                if (drawers[i].TypeEquals(type))
                {
                    value = drawers[i].DrawAndGetNewValue(type, value, draw, field);
                    return value;
                }
            }

            ShowUnrecognized(name);
            return value;
        }

        public static object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
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

        public static object CreateInstance(int index)
        {
            if (index >= 0 && index < selectTypes.Count)
            {
                return CreateInstance(selectTypes[index]);
            }

            Debug.LogError($"selectTypes.Count={selectTypes.Count};index={index}");
            return null;
        }

        public static object CreateInstance(Type type)
        {
            object o = null;

            try
            {
                if (type == typeof(string))
                {
                    o = string.Empty;
                }
                else
                {
                    o = Activator.CreateInstance(type);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return o;
        }

        public static void ShowNull(string name, Type type, ref object value)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(name, "null");
            if (type != null)
            {
                if (GUILayout.Button("new", GUILayout.Width(40)))
                {
                    value = CreateInstance(type);
                }
            }

            EditorGUILayout.EndHorizontal();
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

        #region 收集所有类型

        private static Type[] CollectTypes()
        {
            var h = typeof(Hotfix.Init).Assembly;
            var m = typeof(Model.Game).Assembly;
            var t = typeof(ILRuntime.Runtime.Enviorment.AppDomain).Assembly;
            var u = typeof(GameObject).Assembly;

            List<Type> types = new List<Type>();
            types.AddRange(AssemblyTypes(h));
            types.AddRange(AssemblyTypes(m));
            types.AddRange(AssemblyTypes(t));
            types.AddRange(AssemblyTypes(u));

            return types.ToArray();
        }

        private static Type[] AssemblyTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where((t) =>
            {
                return t.IsDefined(typeof(Model.NewObjectForComponentViewAttribute), false) && !t.IsValueType && !t.IsInterface;
            }).ToArray();
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