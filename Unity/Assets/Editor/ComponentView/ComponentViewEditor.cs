using ILRuntime.Runtime.Intepreter;
using Model;
using ObjectDrawer;
using UnityEditor;

namespace Editors
{
    [CustomEditor(typeof(ComponentView))]
    public class ComponentViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var componentView = (ComponentView) this.target;
            var component = componentView.Component;
            if (component == null || component.GetType() == typeof(ILTypeInstance))
            {
                return;
            }

            ObjectDrawerHelper.Draw(component);
        }
    }
}