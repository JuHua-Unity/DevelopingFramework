﻿#if UNITY_EDITOR
using ILRuntime.Runtime.Intepreter;
using Model;
using UnityEditor;

namespace Editors
{
    [CustomEditor(typeof(ComponentView))]
    public class ComponentViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ComponentView componentView = (ComponentView)target;
            object component = componentView.Component;
            if (component.GetType() == typeof(ILTypeInstance))
            {
                return;
            }

            ComponentViewHelper.Draw(component);
        }
    }
}
#endif