using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class UnityObjectDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, type, true);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            if (type == typeof(UnityEngine.Object))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                return true;
            }

            return false;
        }
    }

    [ComponentViewDrawer]
    internal class Vector2Drawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.Vector2Field(name, (Vector2)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector2);
        }
    }

    [ComponentViewDrawer]
    internal class Vector3Drawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.Vector3Field(name, (Vector3)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector3);
        }
    }

    [ComponentViewDrawer]
    internal class Vector4Drawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.Vector4Field(name, (Vector4)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector4);
        }
    }

    [ComponentViewDrawer]
    internal class RectDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.RectField(name, (Rect)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Rect);
        }
    }

    [ComponentViewDrawer]
    internal class BoundsDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.BoundsField(name, (Bounds)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Bounds);
        }
    }

    [ComponentViewDrawer]
    internal class ColorDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            value = EditorGUILayout.ColorField(name, (Color)value);
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Color);
        }
    }

    [ComponentViewDrawer]
    internal class AnimationCurveDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            if (value == null)
            {
                ComponentViewHelper.NullShow(name);
            }
            else
            {
                value = EditorGUILayout.CurveField(name, (AnimationCurve)value);
            }
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(AnimationCurve);
        }
    }

    [ComponentViewDrawer]
    internal class GradientDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!changeable);
            if (value == null)
            {
                ComponentViewHelper.NullShow(name);
            }
            else
            {
                value = EditorGUILayout.GradientField(name, (Gradient)value);
            }
            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Gradient);
        }
    }
}