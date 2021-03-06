﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class ObjectDrawer : IObjectDrawer
    {
        private static readonly BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private Type type;
        private string name;
        private bool fold;
        private FieldInfo[] fieldInfos;
        private FieldInfo f;
        private readonly List<FieldInfo> fs = new List<FieldInfo>();

        public int Priority => DrawerPriority.Object;

        public void Draw(object value, FieldInfo field = null)
        {
            this.type = value.GetType();
            this.name = field == null ? this.type.Name : field.Name;
            this.fold = ObjectDrawerHelper.GetAndAddFold(value);
            this.fold = EditorGUILayout.Foldout(this.fold, this.name, true);
            ObjectDrawerHelper.SetAndAddFold(value, this.fold);
            if (!this.fold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            ObjectDrawerHelper.Tab();

            this.fieldInfos = this.type.GetFields(bindingFlags);
            this.fs.Clear();
            for (var i = 0; i < this.fieldInfos.Length; i++)
            {
                this.f = this.fieldInfos[i];
                //剔除值为自己的字段
                if ((this.f.FieldType == this.type || this.type.IsSubclassOf(this.f.FieldType) || this.f.FieldType.IsSubclassOf(this.type)) && this.f.GetValue(value) == value)
                {
                    continue;
                }

                this.fs.Add(this.f);
            }

            EditorGUILayout.BeginVertical();
            if (this.fs.Count > 0)
            {
                for (var i = 0; i < this.fs.Count; i++)
                {
                    this.f = this.fs[i];
                    ObjectDrawerHelper.Draw(this.f.GetValue(value), this.f);
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        public bool TypeEquals(Type t)
        {
            return t != typeof(string) && !t.IsValueType;
        }
    }
}