using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class DelegateDrawer : IObjectDrawer
    {
        private Type type;
        private string name;
        private bool fold;
        private int len;
        private Delegate v;
        private Delegate d;

        public int Priority => DrawerPriority.Delegate;

        public void Draw(object value, FieldInfo field = null)
        {
            type = value.GetType();
            if (field == null)
            {
                name = type.Name;
            }
            else
            {
                name = field.Name;
            }

            v = (Delegate)value;
            fold = ObjectDrawerHelper.GetAndAddFold(name);
            fold = EditorGUILayout.Foldout(fold, name, true);
            ObjectDrawerHelper.SetAndAddFold(name, fold);
            if (!fold)
            {
                return;
            }

            Delegate[] ds = v.GetInvocationList();
            len = ds.Length / 10 + 1;
            for (int i = 0; i < ds.Length; i++)
            {
                d = ds[i];
                EditorGUILayout.BeginHorizontal();
                ObjectDrawerHelper.Tab();
                EditorGUILayout.LabelField("Delegate:", GUILayout.Width(60));
                EditorGUILayout.LabelField($"{i}", GUILayout.Width(len * 5 + 15));
                EditorGUILayout.LabelField($"{d.Method.ReflectedType}.{d.Method.Name}");
                EditorGUILayout.EndHorizontal();
            }
        }

        public bool TypeEquals(Type type)
        {
            if (type == typeof(Delegate))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(Delegate)))
            {
                return true;
            }

            return typeof(Delegate).IsAssignableFrom(type);
        }
    }
}