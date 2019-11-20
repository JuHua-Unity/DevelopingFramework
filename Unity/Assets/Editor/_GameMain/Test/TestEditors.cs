using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ILRuntime.CLR.Utils;
using System.IO;
using System.Text;

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
    }
}