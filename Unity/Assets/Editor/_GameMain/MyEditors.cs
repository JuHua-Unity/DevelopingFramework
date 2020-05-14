// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

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

        [MenuItem("Assets/菜单栏/路径")]
        private static void Func_Test()
        {
            TestEditors.ShowPath();
        }

        [MenuItem("菜单栏/测试")]
        private static void Func_Test1()
        {
            TestEditors.TestSth();
        }

        [MenuItem("菜单栏/测试窗口")]
        private static void Func_TestWindows()
        {
            TestEditorWindows.Open();
        }
    }
}