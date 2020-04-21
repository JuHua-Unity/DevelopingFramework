using UnityEditor;

namespace ObjectDrawer
{
    [CustomEditor(typeof(ObjectView))]
    public class ObjectViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ObjectDrawerHelper.Draw(((ObjectView) this.target).Obj);
        }
    }
}