using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Editors
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
            type = value.GetType();
            if (field == null)
            {
                name = type.Name;
            }
            else
            {
                name = field.Name;
            }

            fold = ObjectDrawerHelper.GetAndAddFold(name);
            fold = EditorGUILayout.Foldout(fold, name, true);
            ObjectDrawerHelper.SetAndAddFold(name, fold);
            if (!fold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            ObjectDrawerHelper.Tab();

            fieldInfos = type.GetFields(bindingFlags);
            fs.Clear();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                //剔除类型为自己(父类、子类这种有继承关系)的字段
                f = fieldInfos[i];
                if (f.FieldType != type && !type.IsSubclassOf(f.FieldType) && !f.FieldType.IsSubclassOf(type))
                {
                    fs.Add(f);
                }
            }

            EditorGUILayout.BeginVertical();
            if (fs.Count > 0)
            {
                for (int i = 0; i < fs.Count; i++)
                {
                    f = fs[i];
                    ObjectDrawerHelper.Draw(f.GetValue(value), f);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        public bool TypeEquals(Type type)
        {
            return type != typeof(string) && !type.IsValueType;
        }
    }
}