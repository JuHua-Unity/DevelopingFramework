using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [ComponentViewDrawer]
    internal class UnityObjectDrawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class Vector2Drawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class Vector3Drawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class Vector4Drawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class RectDrawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class BoundsDrawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class ColorDrawer : IComponentViewDrawer
    {
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

    [ComponentViewDrawer]
    internal class AnimationCurveDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.ShowName);
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
                    ComponentViewHelper.ShowNull(draw.ShowName);
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
                    ComponentViewHelper.ShowNull(draw.ShowName);
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

    [ComponentViewDrawer]
    internal class GradientDrawer : IComponentViewDrawer
    {
        public object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field)
        {
            EditorGUI.BeginDisabledGroup(!draw.Changeable);

            if (draw.ShowNameWidth < 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.ShowName);
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
                    ComponentViewHelper.ShowNull(draw.ShowName);
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
                    ComponentViewHelper.ShowNull(draw.ShowName);
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