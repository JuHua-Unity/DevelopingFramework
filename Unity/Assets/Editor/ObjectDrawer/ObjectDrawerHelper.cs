using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    internal static class ObjectDrawerHelper
    {
        private static readonly List<IObjectDrawer> drawers = null;//排好序的drawer

        private static Type type;

        static ObjectDrawerHelper()
        {
            drawers = new List<IObjectDrawer>();
            Assembly assembly = typeof(ObjectDrawerHelper).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsDefined(typeof(ObjectDrawerAttribute)))
                {
                    continue;
                }

                IObjectDrawer drawer = GetDrawer(type);
                drawers.Add(drawer);
                ReturnDrawer(drawer);
            }

            drawers.Sort((a, b) => { return a.Priority.CompareTo(b.Priority); });
        }

        public static void Draw(object obj, FieldInfo field = null)
        {
            if (drawers == null || drawers.Count < 1)
            {
                return;
            }

            EditorGUI.BeginDisabledGroup(true);

            if (obj == null)
            {
                if (field != null)
                {
                    EditorGUILayout.LabelField(field.Name, $"Null");
                }
                else
                {
                    EditorGUILayout.LabelField($"Null");
                }

                EditorGUI.EndDisabledGroup();
                return;
            }

            type = obj.GetType();
            for (int i = 0; i < drawers.Count; i++)
            {
                if (!drawers[i].TypeEquals(type))
                {
                    continue;
                }

                IObjectDrawer d = GetDrawer(drawers[i].GetType());
                EditorGUILayout.BeginVertical();
                d.Draw(obj, field);
                EditorGUILayout.EndVertical();
                ReturnDrawer(d);
                break;
            }

            EditorGUI.EndDisabledGroup();
        }

        #region DrawerPool

        private static readonly Dictionary<Type, List<IObjectDrawer>> drawerPool = new Dictionary<Type, List<IObjectDrawer>>();

        private static IObjectDrawer GetDrawer(Type type)
        {
            IObjectDrawer drawer = null;
            if (drawerPool.ContainsKey(type) && drawerPool[type] != null && drawerPool[type].Count > 0)
            {
                drawer = drawerPool[type][0];
                drawerPool[type].RemoveAt(0);
            }
            else
            {
                //Debug.Log($"创建一个Drawer：{type}");
                drawer = (IObjectDrawer)Activator.CreateInstance(type);
            }

            return drawer;
        }

        private static void ReturnDrawer(IObjectDrawer drawer)
        {
            Type type = drawer.GetType();
            if (drawerPool.ContainsKey(type))
            {
                if (drawerPool[type] == null)
                {
                    drawerPool[type] = new List<IObjectDrawer>();
                }

                drawerPool[type].Add(drawer);
            }
            else
            {
                drawerPool.Add(type, new List<IObjectDrawer>() { drawer });
            }
        }

        #endregion

        #region Tab

        public static void Tab()
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20));
        }

        #endregion

        #region foldouts

        private static readonly Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        public static bool GetAndAddFold(string name)
        {
            if (!foldouts.ContainsKey(name))
            {
                foldouts.Add(name, false);
            }

            return foldouts[name];
        }

        public static void SetAndAddFold(string name, bool fold)
        {
            if (!foldouts.ContainsKey(name))
            {
                foldouts.Add(name, false);
            }

            foldouts[name] = fold;
        }

        #endregion
    }
}