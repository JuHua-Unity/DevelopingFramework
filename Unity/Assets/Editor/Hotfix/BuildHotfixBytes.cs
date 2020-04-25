using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [InitializeOnLoad]
    internal class BuildHotfixBytes
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/_GameMain/Res/Code/";
        private const string HotfixDll = "Hotfix.dll";
        private const string HotfixPdb = "Hotfix.pdb";

        /// <summary>
        /// 复制Library里面的热更代码DLL到工程中
        /// </summary>
        static BuildHotfixBytes()
        {
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Debug.Log($"复制Hotfix.dll, Hotfix.pdb到_GameMain/Res/Code完成");
            AssetDatabase.Refresh();
        }
    }
}