using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class TestEditors : Editor
    {
        public static void Path()
        {
            var obj = Selection.activeObject;
            if (obj != null)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                Debug.Log(path);
            }
        }
    }
}