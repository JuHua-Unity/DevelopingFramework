using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ObjectDrawer
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
            this.type = value.GetType();
            this.name = field == null ? this.type.Name : field.Name;
            this.v = (ICollection) value;
            this.fold = ObjectDrawerHelper.GetAndAddFold(this.name);
            this.fold = EditorGUILayout.Foldout(this.fold, this.name, true);
            ObjectDrawerHelper.SetAndAddFold(this.name, this.fold);
            if (!this.fold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("数量：", GUILayout.Width(60));
            EditorGUILayout.LabelField(this.v.Count.ToString());
            EditorGUILayout.EndHorizontal();

            this.list.Clear();
            foreach (var item in this.v)
            {
                this.list.Add(item);
            }

            for (var i = 0; i < this.list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ObjectDrawerHelper.Tab();
                EditorGUILayout.LabelField("Element:", GUILayout.Width(60));
                ObjectDrawerHelper.Draw(this.list[i]);
                EditorGUILayout.EndHorizontal();
            }
        }

        public bool TypeEquals(Type t)
        {
            return typeof(ICollection).IsAssignableFrom(t);
        }
    }
}