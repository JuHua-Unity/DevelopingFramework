// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

using UnityEditor;

namespace Editors
{
    public class CustomEditors : Editor
    {
        [MenuItem("Assets/菜单栏/设置路径下所有文件(除文件夹)的AB名字")]
        private static void Func1()
        {
            AssetBundleNameEditor.SetAssetBundleName();
        }

        [MenuItem("Assets/菜单栏/清除选中文件夹及文件的AB名字")]
        private static void Func2()
        {
            AssetBundleNameEditor.ClearAssetBundleName();
        }

        [MenuItem("Assets/菜单栏/清除路径下所有文件夹及文件的AB名字")]
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