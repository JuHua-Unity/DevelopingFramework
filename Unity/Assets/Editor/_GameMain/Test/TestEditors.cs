using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class TestEditors : Editor
    {
        public static void ShowPath()
        {
            var obj = Selection.activeObject;
            if (obj != null)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                Debug.Log(path);
            }
        }

        public static void TestSth()
        {
            Debug.Log($"CurrentDirectory:{System.IO.Directory.GetCurrentDirectory()}");
            Debug.Log($"Application.dataPath:{Application.dataPath}");
            Debug.Log($"Application.persistentDataPath:{Application.persistentDataPath}");
            Debug.Log($"Application.streamingAssetsPath:{Application.streamingAssetsPath}");
            Debug.Log($"Application.temporaryCachePath:{Application.temporaryCachePath}");
        }
    }
}