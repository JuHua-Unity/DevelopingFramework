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

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.ObjectField(draw.FieldName, (UnityEngine.Object)value, type, true);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.ObjectField((UnityEngine.Object)value, type, true);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
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
            value = EditorGUILayout.Vector2Field(draw.FieldName, (Vector2)value);
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
            value = EditorGUILayout.Vector3Field(draw.FieldName, (Vector3)value);
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
            value = EditorGUILayout.Vector4Field(draw.FieldName, (Vector4)value);
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
            value = EditorGUILayout.RectField(draw.FieldName, (Rect)value);
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

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.BoundsField(draw.FieldName, (Bounds)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.BoundsField((Bounds)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
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

            if (draw.FieldNameWidth < 0)
            {
                value = EditorGUILayout.ColorField(draw.FieldName, (Color)value);
            }
            else if (draw.FieldNameWidth == 0)
            {
                value = EditorGUILayout.ColorField((Color)value);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
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

            if (draw.FieldNameWidth < 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
                }
                else
                {
                    value = EditorGUILayout.CurveField(draw.FieldName, (AnimationCurve)value);
                }
            }
            else if (draw.FieldNameWidth == 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
                }
                else
                {
                    value = EditorGUILayout.CurveField((AnimationCurve)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
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

            if (draw.FieldNameWidth < 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
                }
                else
                {
                    value = EditorGUILayout.GradientField(draw.FieldName, (Gradient)value);
                }
            }
            else if (draw.FieldNameWidth == 0)
            {
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
                }
                else
                {
                    value = EditorGUILayout.GradientField((Gradient)value);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(draw.FieldName, GUILayout.Width(draw.FieldNameWidth));
                if (value == null)
                {
                    ComponentViewHelper.ShowNull(draw.FieldName);
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