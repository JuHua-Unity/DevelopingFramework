using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class UnityObjectDrawer : IObjectDrawer
    {
        private Type type;

        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            type = value.GetType();
            if (field == null)
            {
                EditorGUILayout.ObjectField((UnityEngine.Object)value, type, true);
            }
            else
            {
                EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object)value, type, true);
            }
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

    [ObjectDrawer]
    internal class Vector2Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.Vector2Field("Vector2", (Vector2)value);
            }
            else
            {
                EditorGUILayout.Vector2Field(field.Name, (Vector2)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector2);
        }
    }

    [ObjectDrawer]
    internal class Vector3Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.Vector3Field("Vector3", (Vector3)value);
            }
            else
            {
                EditorGUILayout.Vector3Field(field.Name, (Vector3)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector3);
        }
    }

    [ObjectDrawer]
    internal class Vector4Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.Vector4Field("Vector4", (Vector4)value);
            }
            else
            {
                EditorGUILayout.Vector4Field(field.Name, (Vector4)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Vector4);
        }
    }

    [ObjectDrawer]
    internal class RectDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.RectField((Rect)value);
            }
            else
            {
                EditorGUILayout.RectField(field.Name, (Rect)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Rect);
        }
    }

    [ObjectDrawer]
    internal class BoundsDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.BoundsField((Bounds)value);
            }
            else
            {
                EditorGUILayout.BoundsField(field.Name, (Bounds)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Bounds);
        }
    }

    [ObjectDrawer]
    internal class ColorDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.ColorField((Color)value);
            }
            else
            {
                EditorGUILayout.ColorField(field.Name, (Color)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Color);
        }
    }

    [ObjectDrawer]
    internal class AnimationCurveDrawer : IObjectDrawer
    {
        private Type type;
        private string name;

        public int Priority => DrawerPriority.Zero;

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

            if (field == null)
            {
                EditorGUILayout.CurveField((AnimationCurve)value);
            }
            else
            {
                EditorGUILayout.CurveField(field.Name, (AnimationCurve)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(AnimationCurve);
        }
    }

    [ObjectDrawer]
    internal class GradientDrawer : IObjectDrawer
    {
        private Type type;
        private string name;

        public int Priority => DrawerPriority.Zero;

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

            if (field == null)
            {
                EditorGUILayout.GradientField((Gradient)value);
            }
            else
            {
                EditorGUILayout.GradientField(field.Name, (Gradient)value);
            }
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Gradient);
        }
    }
}