using UnityEditor;

namespace Editors
{
    public class MyEditors : Editor
    {
        [MenuItem("菜单栏/启动参数配置")]
        private static void Func1()
        {
            LaunchOptionsEditor.Open();
        }

        [MenuItem("Assets/菜单栏/清除AB名字")]
        private static void Func2()
        {
            AssetBundleNameEditor.ClearAssetBundleName(SelectionMode.Assets);
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

        [MenuItem("菜单栏/Core/宏定义")]
        private static void Func5()
        {
            DefineWindow.Open();
        }

        [MenuItem("菜单栏/Core/AssetBundle")]
        private static void Func6()
        {
            AssetBundlesEditor.Open();
        }

        [MenuItem("菜单栏/Core/Builder")]
        private static void Func7()
        {
            BuildPlayerEditor.Open();
        }

        [MenuItem("Assets/菜单栏/路径")]
        private static void Func_Test()
        {
            TestEditors.ShowPath();
        }

        [MenuItem("菜单栏/查找委托")]
        private static void Func_FindDelegate()
        {
            CollectDelegate.FindDelegate();
        }

        [MenuItem("菜单栏/测试窗口")]
        private static void Func_TestWindows()
        {
            TestEditorWindows.Open();
        }
    }
}