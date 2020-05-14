using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [InitializeOnLoad]
    // ReSharper disable once UnusedMember.Global
    internal class BuildHotfixBytes
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/_GameMain/Res/Code/";

        /// <summary>
        /// 复制Library里面的热更代码DLL到工程中
        /// </summary>
        static BuildHotfixBytes()
        {
            File.Copy(Path.Combine(ScriptAssembliesDir, Model.Define.HotfixDll), Path.Combine(CodeDir, $"{Model.Define.HotfixDll}.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, Model.Define.HotfixPdb), Path.Combine(CodeDir, $"{Model.Define.HotfixPdb}.bytes"), true);
            Debug.Log($"复制Hotfix.dll, Hotfix.pdb到{CodeDir}完成");
            AssetDatabase.Refresh();
        }
    }
}