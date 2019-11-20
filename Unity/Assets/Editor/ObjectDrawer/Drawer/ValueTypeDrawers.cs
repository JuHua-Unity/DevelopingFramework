using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class IntDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedIntField(draw.ShowName, (int)value);
                }
                else
                {
                    value = EditorGUILayout.IntField(draw.ShowName, (int)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedIntField((int)value);
                }
                else
                {
                    value = EditorGUILayout.IntField((int)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedIntField((int)value);
                }
                else
                {
                    value = EditorGUILayout.IntField((int)value);
                }

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

    [ObjectDrawer]
    internal class FloatDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedFloatField(draw.ShowName, (float)value);
                }
                else
                {
                    value = EditorGUILayout.FloatField(draw.ShowName, (float)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedFloatField((float)value);
                }
                else
                {
                    value = EditorGUILayout.FloatField((float)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedFloatField((float)value);
                }
                else
                {
                    value = EditorGUILayout.FloatField((float)value);
                }

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

    [ObjectDrawer]
    internal class BoolDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                value = EditorGUILayout.Toggle(draw.ShowName, (bool)value);
            }
            else if (draw.ShowNameWidth == 0)
            {
                value = EditorGUILayout.Toggle((bool)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
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

    [ObjectDrawer]
    internal class LongDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                value = EditorGUILayout.LongField(draw.ShowName, (long)value);
            }
            else if (draw.ShowNameWidth == 0)
            {
                value = EditorGUILayout.LongField((long)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
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

    [ObjectDrawer]
    internal class DoubleDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedDoubleField(draw.ShowName, (double)value);
                }
                else
                {
                    value = EditorGUILayout.DoubleField(draw.ShowName, (double)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedDoubleField((double)value);
                }
                else
                {
                    value = EditorGUILayout.DoubleField((double)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    value = EditorGUILayout.DelayedDoubleField((double)value);
                }
                else
                {
                    value = EditorGUILayout.DoubleField((double)value);
                }

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

    [ObjectDrawer]
    internal class ByteDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (byte)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class CharDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (char)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class DecimalDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (decimal)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class SbyteDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (sbyte)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class ShortDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (short)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class UintDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (uint)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class UlongDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (ulong)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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

    [ObjectDrawer]
    internal class UshortDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            var v = (ushort)value;
            string str = v.ToString();
            if (draw.ShowNameWidth < 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(draw.ShowName, str);
                }
                else
                {
                    str = EditorGUILayout.TextField(draw.ShowName, str);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (draw.NeedDelayed)
                {
                    str = EditorGUILayout.DelayedTextField(str);
                }
                else
                {
                    str = EditorGUILayout.TextField(str);
                }

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