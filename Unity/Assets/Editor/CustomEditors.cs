// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

using UnityEditor;

namespace Editors
{
    public class CustomEditors : Editor
    {
        [MenuItem("Assets/菜单栏/设置AB名字")]
        private static void Func1()
        {
            AssetBundleNameEditor.SetAssetBundleName();
        }

        [MenuItem("Assets/菜单栏/清除AB名字")]
        private static void Func2()
        {
            AssetBundleNameEditor.ClearAssetBundleName();
        }

        [MenuItem("Assets/菜单栏/清除AB名字(包含子文件)")]
        private static void Func3()
        {
            AssetBundleNameEditor.ClearAssetBundleName(SelectionMode.DeepAssets);
        }

        [MenuItem("菜单栏/查看所有AB名字")]
        private static void Func4()
        {
            AssetBundleNameEditor.FindAllAssetBundleName();
        }

        [MenuItem("菜单栏/Core/宏定义", priority = -1)]
        private static void Func5()
        {
            ScriptingDefineSymbolsWindow.Open();
        }

        [MenuItem("菜单栏/Core/AssetBunles", priority = -1)]
        private static void Func6()
        {
            AssetBundlesEditor.Open();
        }

        [MenuItem("菜单栏/Core/Builder", priority = -1)]
        private static void Func7()
        {
            BuildWindow.Open();
        }
    }
}