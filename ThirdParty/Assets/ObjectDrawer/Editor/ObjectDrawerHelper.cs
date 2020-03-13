using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ObjectDrawer
{
    public static class ObjectDrawerHelper
    {
        private static readonly List<IObjectDrawer> drawers; //排好序的drawer

        private static Type type;

        static ObjectDrawerHelper()
        {
            drawers = new List<IObjectDrawer>();
            var assembly = typeof(ObjectDrawerHelper).Assembly;
            foreach (var t in assembly.GetTypes())
            {
                if (!t.IsDefined(typeof(ObjectDrawerAttribute)))
                {
                    continue;
                }

                var drawer = GetDrawer(t);
                drawers.Add(drawer);
                ReturnDrawer(drawer);
            }

            drawers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
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
                    EditorGUILayout.LabelField(field.Name, "Null");
                }
                else
                {
                    EditorGUILayout.LabelField("Null");
                }

                EditorGUI.EndDisabledGroup();
                return;
            }

            type = obj.GetType();
            for (var i = 0; i < drawers.Count; i++)
            {
                if (!drawers[i].TypeEquals(type))
                {
                    continue;
                }

                var d = GetDrawer(drawers[i].GetType());
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

        private static IObjectDrawer GetDrawer(Type t)
        {
            IObjectDrawer drawer;
            if (drawerPool.ContainsKey(t) && drawerPool[t] != null && drawerPool[t].Count > 0)
            {
                drawer = drawerPool[t][0];
                drawerPool[t].RemoveAt(0);
            }
            else
            {
                //Debug.Log($"创建一个Drawer：{type}");
                drawer = (IObjectDrawer) Activator.CreateInstance(t);
            }

            return drawer;
        }

        private static void ReturnDrawer(IObjectDrawer drawer)
        {
            var t = drawer.GetType();
            if (drawerPool.ContainsKey(t))
            {
                if (drawerPool[t] == null)
                {
                    drawerPool[t] = new List<IObjectDrawer>();
                }

                drawerPool[t].Add(drawer);
            }
            else
            {
                drawerPool.Add(t, new List<IObjectDrawer>() {drawer});
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