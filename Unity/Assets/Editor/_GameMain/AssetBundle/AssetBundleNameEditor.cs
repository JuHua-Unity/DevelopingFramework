using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class AssetBundleNameEditor : Editor
    {
        public static void ClearAssetBundleName(SelectionMode selectionMode = SelectionMode.Assets)
        {
            var objs = Selection.GetFiltered<Object>(selectionMode);
            if (objs == null || objs.Length < 1)
            {
                return;
            }

            for (int i = 0; i < objs.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(objs[i]);
                var importor = AssetImporter.GetAtPath(path);
                if (!string.IsNullOrEmpty(importor.assetBundleName))
                {
                    importor.assetBundleName = null;
                    Debug.Log($"清除[{path}]assetBundleName名字完成！");
                }

                if (!string.IsNullOrEmpty(importor.assetBundleVariant))
                {
                    importor.assetBundleVariant = null;
                    Debug.Log($"清除[{path}]assetBundleVariant完成！");
                }

                importor.SaveAndReimport();
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
            Debug.Log("清除AB名字完成！");
        }

        public static void FindAllAssetBundleName()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < names.Length; i++)
            {
                Debug.Log($"AssetBundleName:{names[i]},包含{string.Join(";", AssetDatabase.GetAssetPathsFromAssetBundle(names[i]))}");
            }

            Debug.Log("查看所有AB名字完成！");
        }
    }
}