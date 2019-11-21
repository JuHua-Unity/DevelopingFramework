using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ObjectDrawer]
    internal class UnityObjectDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                value = EditorGUILayout.ObjectField(draw.ShowName, (UnityEngine.Object)value, type, true);
            }
            else if (draw.ShowNameWidth == 0)
            {
                value = EditorGUILayout.ObjectField((UnityEngine.Object)value, type, true);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                value = EditorGUILayout.ObjectField((UnityEngine.Object)value, type, true);

                EditorGUILayout.EndHorizontal();
            }

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

    [ObjectDrawer]
    internal class Vector2Drawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);
            value = EditorGUILayout.Vector2Field(draw.ShowName, (Vector2)value);
            EditorGUI.EndDisabledGroup();
            return value;
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

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);
            value = EditorGUILayout.Vector3Field(draw.ShowName, (Vector3)value);
            EditorGUI.EndDisabledGroup();
            return value;
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

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);
            value = EditorGUILayout.Vector4Field(draw.ShowName, (Vector4)value);
            EditorGUI.EndDisabledGroup();
            return value;
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

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);
            value = EditorGUILayout.RectField(draw.ShowName, (Rect)value);
            EditorGUI.EndDisabledGroup();
            return value;
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

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                value = EditorGUILayout.BoundsField(draw.ShowName, (Bounds)value);
            }
            else if (draw.ShowNameWidth == 0)
            {
                value = EditorGUILayout.BoundsField((Bounds)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                value = EditorGUILayout.BoundsField((Bounds)value);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
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

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                value = EditorGUILayout.ColorField(draw.ShowName, (Color)value);
            }
            else if (draw.ShowNameWidth == 0)
            {
                value = EditorGUILayout.ColorField((Color)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                value = EditorGUILayout.ColorField((Color)value);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(Color);
        }
    }

    [ObjectDrawer]
    internal class AnimationCurveDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.CurveField(draw.ShowName, (AnimationCurve)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.CurveField((AnimationCurve)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.CurveField((AnimationCurve)value);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            return value;
        }

        public bool TypeEquals(Type type)
        {
            return type == typeof(AnimationCurve);
        }
    }

    [ObjectDrawer]
    internal class GradientDrawer : IObjectDrawer
    {
        public int Priority => DrawerPriority.Zero;

        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.GradientField(draw.ShowName, (Gradient)value);
                }
            }
            else if (draw.ShowNameWidth == 0)
            {
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.GradientField((Gradient)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.ShowName, GUILayout.Width(draw.ShowNameWidth));
                if (value == null)
                {
                    ObjectDrawerHelper.ShowNull(draw.ShowName, type, ref value);
                }
                else
                {
                    value = EditorGUILayout.GradientField((Gradient)value);
                }

                EditorGUILayout.EndHorizontal();
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