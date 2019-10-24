using System;
using System.Reflection;
using UnityEditor;

namespace Editors
{
    [ComponentViewDrawer]
    internal class IntDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.IntField(name, (int)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(int);
        }
    }

    [ComponentViewDrawer]
    internal class FloatDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.FloatField(name, (float)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(float);
        }
    }

    [ComponentViewDrawer]
    internal class BoolDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.Toggle(name, (bool)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(bool);
        }
    }

    [ComponentViewDrawer]
    internal class LongDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.LongField(name, (long)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(long);
        }
    }

    [ComponentViewDrawer]
    internal class DoubleDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.DoubleField(name, (double)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(double);
        }
    }

    [ComponentViewDrawer]
    internal class ByteDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (byte)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            byte.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(byte);
        }
    }

    [ComponentViewDrawer]
    internal class CharDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (char)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            char.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(char);
        }
    }

    [ComponentViewDrawer]
    internal class DecimalDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (decimal)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            decimal.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(decimal);
        }
    }

    [ComponentViewDrawer]
    internal class SbyteDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (sbyte)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            sbyte.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(sbyte);
        }
    }

    [ComponentViewDrawer]
    internal class ShortDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (short)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            short.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(short);
        }
    }

    [ComponentViewDrawer]
    internal class UintDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (uint)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            uint.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(uint);
        }
    }

    [ComponentViewDrawer]
    internal class UlongDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (ulong)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            ulong.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(ulong);
        }
    }

    [ComponentViewDrawer]
    internal class UshortDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            var v = (ushort)value;
            string str = EditorGUILayout.DelayedTextField(name, v.ToString());
            ushort.TryParse(str, out v);
            value = v;
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(ushort);
        }
    }
}