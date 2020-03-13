using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectDrawer
{
    [ObjectDrawer]
    internal class UnityObjectDrawer : IObjectDrawer
    {
        private Type type;

        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            this.type = value.GetType();
            if (field == null)
            {
                EditorGUILayout.ObjectField((Object) value, this.type, true);
            }
            else
            {
                EditorGUILayout.ObjectField(field.Name, (Object) value, this.type, true);
            }
        }

        public bool TypeEquals(Type t)
        {
            if (t == typeof(Object))
            {
                return true;
            }

            if (t.IsSubclassOf(typeof(Object)))
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
            EditorGUILayout.Vector2Field(field == null ? "Vector2" : field.Name, (Vector2) value);
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Vector2);
        }
    }

    [ObjectDrawer]
    internal class Vector3Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            EditorGUILayout.Vector3Field(field == null ? "Vector3" : field.Name, (Vector3) value);
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Vector3);
        }
    }

    [ObjectDrawer]
    internal class Vector4Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            EditorGUILayout.Vector4Field(field == null ? "Vector4" : field.Name, (Vector4) value);
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Vector4);
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
                EditorGUILayout.RectField((Rect) value);
            }
            else
            {
                EditorGUILayout.RectField(field.Name, (Rect) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Rect);
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
                EditorGUILayout.BoundsField((Bounds) value);
            }
            else
            {
                EditorGUILayout.BoundsField(field.Name, (Bounds) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Bounds);
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
                EditorGUILayout.ColorField((Color) value);
            }
            else
            {
                EditorGUILayout.ColorField(field.Name, (Color) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Color);
        }
    }

    [ObjectDrawer]
    internal class AnimationCurveDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.CurveField((AnimationCurve) value);
            }
            else
            {
                EditorGUILayout.CurveField(field.Name, (AnimationCurve) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(AnimationCurve);
        }
    }

    [ObjectDrawer]
    internal class GradientDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public void Draw(object value, FieldInfo field = null)
        {
            if (field == null)
            {
                EditorGUILayout.GradientField((Gradient) value);
            }
            else
            {
                EditorGUILayout.GradientField(field.Name, (Gradient) value);
            }
        }

        public bool TypeEquals(Type t)
        {
            return t == typeof(Gradient);
        }
    }
}