using UnityEditor;

namespace Editors
{
    public class CoreEditors : Editor
    {
        [MenuItem("菜单栏/宏定义/[Setting]")]
        private static void Func1()
        {
            DefineWindow.Open();
        }

        [MenuItem("菜单栏/启动参数配置")]
        private static void Func2()
        {
            LaunchOptionsEditor.Open();
        }

        [MenuItem("菜单栏/路径")]
        private static void Func_Test()
        {
            TestEditors.Path();
        }
    }
}