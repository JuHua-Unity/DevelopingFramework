using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class IntDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.DelayedIntField(draw.FieldName, (int)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.DelayedIntField((int)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.DelayedIntField((int)value);

                EditorGUILayout.EndHorizontal();
            }

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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.DelayedFloatField(draw.FieldName, (float)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.DelayedFloatField((float)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.DelayedFloatField((float)value);

                EditorGUILayout.EndHorizontal();
            }

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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.Toggle(draw.FieldName, (bool)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.Toggle((bool)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.Toggle((bool)value);

                EditorGUILayout.EndHorizontal();
            }

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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.LongField(draw.FieldName, (long)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.LongField((long)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.LongField((long)value);

                EditorGUILayout.EndHorizontal();
            }

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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.DelayedDoubleField(draw.FieldName, (double)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.DelayedDoubleField((double)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                value = EditorGUILayout.DelayedDoubleField((double)value);

                EditorGUILayout.EndHorizontal();
            }

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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (byte)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (char)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (decimal)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (sbyte)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (short)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (uint)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (ulong)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (ushort)value;
            string str = v.ToString();
            if (draw.FieldNameWidth < 0)
            {
                str = EditorGUILayout.DelayedTextField(draw.FieldName, str);
            }
            else if (draw.FieldNameWidth == 0)
            {
                str = EditorGUILayout.DelayedTextField(str);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                str = EditorGUILayout.DelayedTextField(str);

                EditorGUILayout.EndHorizontal();
            }
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