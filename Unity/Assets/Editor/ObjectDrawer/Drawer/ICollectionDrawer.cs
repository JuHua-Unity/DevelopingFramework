using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class ICollectionDrawer : IObjectDrawer
    {
        private Type type;
        private string name;
        private ICollection v;
        private bool fold;
        private Type[] types;
        private readonly List<object> list = new List<object>();

        public int Priority => DrawerPriority.Collection;

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

            v = (ICollection)value;
            fold = ObjectDrawerHelper.GetAndAddFold(name);
            fold = EditorGUILayout.Foldout(fold, name, true);
            ObjectDrawerHelper.SetAndAddFold(name, fold);
            if (!fold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
            EditorGUILayout.LabelField(v.Count.ToString());
            EditorGUILayout.EndHorizontal();

            list.Clear();
            foreach (var item in v)
            {
                list.Add(item);
            }

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ObjectDrawerHelper.Tab();
                EditorGUILayout.LabelField("Element:", GUILayout.Width(60));
                ObjectDrawerHelper.Draw(list[i], null);
                EditorGUILayout.EndHorizontal();
            }
        }

        public bool TypeEquals(Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type);
        }
    }
}