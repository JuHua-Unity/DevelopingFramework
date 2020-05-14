using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class AssetBundleNameEditor : Editor
    {
        public static void SetAssetBundleName()
        {
            var objects = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
            if (objects == null || objects.Length < 1)
            {
                return;
            }

            for (var i = 0; i < objects.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(objects[i]);
                if (!File.Exists(path))
                {
                    continue;
                }

                var abName = Path.GetFileNameWithoutExtension(path)?.ToLower();
                if (string.IsNullOrEmpty(abName))
                {
                    Debug.LogError($"[{path}]的无后缀文件名是空的！");
                    continue;
                }

                var import = AssetImporter.GetAtPath(path);
                if (import.assetBundleName != abName)
                {
                    import.assetBundleName = abName;
                    Debug.Log($"设置[{path}]assetBundleName名字完成！");
                }

                if (import.assetBundleVariant != Model.Define.ABVariant)
                {
                    import.assetBundleVariant = Model.Define.ABVariant;
                    Debug.Log($"设置[{path}]assetBundleVariant完成！");
                }

                import.SaveAndReimport();
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
            Debug.Log("设置AB名字完成！");
        }

        public static void ClearAssetBundleName(SelectionMode selectionMode = SelectionMode.Assets)
        {
            var objects = Selection.GetFiltered<Object>(selectionMode);
            if (objects == null || objects.Length < 1)
            {
                return;
            }

            for (var i = 0; i < objects.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(objects[i]);
                var import = AssetImporter.GetAtPath(path);
                if (import.assetBundleName != null)
                {
                    import.assetBundleName = null;
                    Debug.Log($"清除[{path}]assetBundleName名字完成！");
                }

                if (import.assetBundleVariant != null)
                {
                    import.assetBundleVariant = null;
                    Debug.Log($"清除[{path}]assetBundleVariant完成！");
                }

                import.SaveAndReimport();
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
            Debug.Log("清除AB名字完成！");
        }

        public static void FindAllAssetBundleName()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            for (var i = 0; i < names.Length; i++)
            {
                Debug.Log($"AssetBundleName:{names[i]},包含{string.Join(";", AssetDatabase.GetAssetPathsFromAssetBundle(names[i]))}");
            }

            Debug.Log("查看所有AB名字完成！");
        }
    }
}