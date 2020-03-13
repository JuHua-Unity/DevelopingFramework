using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ObjectDrawer
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
            this.type = value.GetType();
            this.name = field == null ? this.type.Name : field.Name;
            this.v = (Delegate) value;
            this.fold = ObjectDrawerHelper.GetAndAddFold(this.name);
            this.fold = EditorGUILayout.Foldout(this.fold, this.name, true);
            ObjectDrawerHelper.SetAndAddFold(this.name, this.fold);
            if (!this.fold)
            {
                return;
            }

            var ds = this.v.GetInvocationList();
            this.len = ds.Length / 10 + 1;
            for (var i = 0; i < ds.Length; i++)
            {
                this.d = ds[i];
                EditorGUILayout.BeginHorizontal();
                ObjectDrawerHelper.Tab();
                EditorGUILayout.LabelField("Delegate:", GUILayout.Width(60));
                EditorGUILayout.LabelField($"{i}", GUILayout.Width(this.len * 5 + 15));
                EditorGUILayout.LabelField($"{this.d.Method.ReflectedType}.{this.d.Method.Name}");
                EditorGUILayout.EndHorizontal();
            }
        }

        public bool TypeEquals(Type t)
        {
            if (t == typeof(Delegate))
            {
                return true;
            }

            if (t.IsSubclassOf(typeof(Delegate)))
            {
                return true;
            }

            return typeof(Delegate).IsAssignableFrom(t);
        }
    }
}