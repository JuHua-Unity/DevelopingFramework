#if ILRuntime && !DEFINE_HOTFIXEDITOR
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public static class ILRuntimeCLRBinding
    {
        private static readonly string path = "Assets/Model/Core/ILRuntime/ILBinding";

        [MenuItem("Tools/ILRuntime/Generate CLR Binding Code by Analysis")]
        public static void GenerateCLRBindingByAnalysis()
        {
            GenerateCLRBinding();

            //用新的分析热更dll调用引用来生成绑定代码
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
            using (FileStream fs = new FileStream("Assets/_GameMain/Res/Code/Hotfix.dll.bytes", FileMode.Open, FileAccess.Read))
            {
                domain.LoadAssembly(fs);
                //Crossbind Adapter is needed to generate the correct binding code
                ILHelper.InitILRuntime(domain);
                ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, path);
                AssetDatabase.Refresh();
            }
        }

        private static void GenerateCLRBinding()
        {
            List<Type> types = new List<Type>
            {
                typeof(int),
                typeof(float),
                typeof(long),
                typeof(object),
                typeof(string),
                typeof(Array),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Quaternion),
                typeof(GameObject),
                typeof(UnityEngine.Object),
                typeof(Transform),
                typeof(RectTransform),
                typeof(Time),
                typeof(Debug),

                //所有DLL内的类型的真实C#类型都是ILTypeInstance
                typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>)
            };

            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, path);
            AssetDatabase.Refresh();
        }
    }
}
#endif
