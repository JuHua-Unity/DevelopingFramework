using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hotfix;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class BuildWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<BuildWindow>().Show();
        }

        private bool buildABFold = true;
        private bool buildAppFold = true;

        private bool development;
        private string abVersion = "0.0.1";
        private string appVersion = "0.0.1";
        private LocalRes res = LocalRes.复制配置部分_打包版本;

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            EditorGUILayout.Space();
            Build();
            EditorGUILayout.Space();
            BuildAPP();
        }

        private void Build()
        {
            this.buildABFold = EditorGUILayout.Foldout(this.buildABFold, "AssetBundle");
            if (!this.buildABFold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            EditorGUILayout.BeginVertical();
            BuildABGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void BuildABGUI()
        {
            this.abVersion = EditorGUILayout.TextField("Version", this.abVersion);
            this.res = (LocalRes) EditorGUILayout.EnumPopup("资源复制：", this.res);

            if (GUILayout.Button("BuildAssetBundles"))
            {
                RunBuildAB();
            }
        }

        private void RunBuildAB()
        {
            var outPath = $"{Define.BuildPath}{Define.TargetName}/{Model.Define.ABsPathParent}/";
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            BuildAssetBundleOptions options;
            switch (Define.Target)
            {
                case BuildTarget.StandaloneWindows64:
                    options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.StrictMode;
                    break;
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.StrictMode | BuildAssetBundleOptions.ChunkBasedCompression;
                    break;
                default:
                    Debug.LogError($"不支持[{Define.Target}]打包！");
                    return;
            }

            var target = Define.Target;
            BuildPipeline.BuildAssetBundles(outPath, options, target);
            AssetDatabase.Refresh();
            Debug.Log("BuildAssetBundles complete");
            CopyABs(outPath);
            AssetDatabase.Refresh();
            Debug.Log("CopyAssetBundles complete");
        }

        private void CopyABs(string outPath)
        {
            var localABs = GetLocalABs();
            var directory = new DirectoryInfo(outPath);
            var files = directory.GetFiles().Where(f => !f.Extension.Equals(".manifest")).ToList();
            var allABs = files.Select(t => t.Name).ToList();

            var serverPath = GetABServerPath();
            Debug.Log($"Server:{serverPath}");
            var localPath = GetABLocalPath();
            Debug.Log($"Local:{localPath}");

            var localFiles = new List<FileInfo>();
            var allFiles = new List<FileInfo>();
            for (var i = 0; i < files.Count; i++)
            {
                var a = files[i];
                var fileName = a.Name;
                if (localABs.Contains(fileName))
                {
                    localFiles.Add(a.CopyTo(Path.Combine(localPath, fileName)));
                }

                if (allABs.Contains(fileName))
                {
                    allFiles.Add(a.CopyTo(Path.Combine(serverPath, fileName)));
                }
            }

            GenerateVersion(localPath, localFiles);
            GenerateVersion(serverPath, allFiles);
        }

        private static void GenerateVersion(string path, IReadOnlyList<FileInfo> files)
        {
            var count = files.Count;
            var a = new ResVersion();
            if (count > 0)
            {
                a.Res = new ResVersion.ResVersionInfo[count];

                for (var i = 0; i < count; i++)
                {
                    var fileInfo = files[i];
                    a.Res[i] = new ResVersion.ResVersionInfo
                    {
                        File = fileInfo.Name,
                        MD5 = MD5Helper.FileMD5(fileInfo.FullName),
                        Size = fileInfo.Length
                    };
                }
            }

            IOHelper.StreamWriter(JsonMapper.ToJson(a), Path.Combine(path, Model.Define.ResVersionJson));
        }

        private List<string> GetLocalABs()
        {
            List<string> list;
            switch (this.res)
            {
                case LocalRes.不复制_纯热更:
                    list = new List<string>();
                    break;
                case LocalRes.复制配置部分_打包版本:
                    list = (from b in AssetBundlesEditor.GetABs() where b.CopyToLocal select b.Name).ToList();
                    break;
                case LocalRes.复制全部_本地模拟:
                    list = AssetBundlesEditor.GetABs().Select(b => b.Name).ToList();
                    break;
                default:
                    list = new List<string>();
                    break;
            }

            //这个文件每次都复制一下
            list.Add(Model.Define.ABsPathParent);
            return list;
        }

        private string GetABServerPath()
        {
            if (string.IsNullOrEmpty(this.abVersion))
            {
                Debug.LogError("abversion is empty!");
                return null;
            }

            var path = $"{Define.BuildPath}Server/{Define.TargetName}/{this.abVersion}";
            if (Directory.Exists(path))
            {
                Debug.Log($"{path} 已删除！");
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
            return path;
        }

        private string GetABLocalPath()
        {
            var path = $"{Application.streamingAssetsPath}/{Model.Define.ABsPathParent}";
            if (Directory.Exists(path))
            {
                Debug.Log($"{path} 已删除！");
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
            return path;
        }

        private void BuildAPP()
        {
            this.buildAppFold = EditorGUILayout.Foldout(this.buildAppFold, "Build");
            if (!this.buildAppFold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            EditorGUILayout.BeginVertical();
            BuildAppM();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void BuildAppM()
        {
            this.appVersion = EditorGUILayout.TextField("Version", this.appVersion);
            this.development = EditorGUILayout.Toggle("Development", this.development);

            if (GUILayout.Button("BuildPlayer"))
            {
                RunBuildApp();
            }
        }

        private void RunBuildApp()
        {
#if DEFINE_SHOWLOG
            string[] levels = { "Assets/Scenes/Init_Debug.unity", };
#else
            string[] levels = {"Assets/Scenes/Init.unity",};
#endif
            var version = this.appVersion;
            if (string.IsNullOrEmpty(version))
            {
                Debug.LogError("version is empty!");
                return;
            }

            PlayerSettings.bundleVersion = version;

            var outPath = $"{Define.BuildPath}Build/{Define.TargetName}/{version}_{DateTime.Now:yyyy-MM-dd HH-mm-ss}/";
            if (Directory.Exists(outPath))
            {
                Debug.Log($"{outPath} 已删除！");
                Directory.Delete(outPath, true);
            }

            Directory.CreateDirectory(outPath);

            switch (Define.Target)
            {
                case BuildTarget.Android:
                    outPath = $"{outPath}Game.apk";
                    if (File.Exists(outPath))
                    {
                        Debug.Log($"{outPath} 已删除！");
                        File.Delete(outPath);
                    }

                    break;
                case BuildTarget.StandaloneWindows64:
                    outPath = $"{outPath}Game.exe";
                    break;
                case BuildTarget.iOS:
                    outPath = $"{outPath}Game";
                    if (Directory.Exists(outPath))
                    {
                        Debug.Log($"{outPath} 已删除！");
                        Directory.Delete(outPath, true);
                    }

                    Directory.CreateDirectory(outPath);
                    break;
                default:
                    Debug.LogError($"不支持[{Define.Target}]打包！");
                    return;
            }

            var options = BuildOptions.None;
            if (this.development)
            {
                options = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
            }

            var target = Define.Target;
            BuildPipeline.BuildPlayer(levels, outPath, target, options);
            AssetDatabase.Refresh();
            Debug.Log("BuildPlayer complete");
        }

        private enum LocalRes
        {
            不复制_纯热更,
            复制配置部分_打包版本,
            复制全部_本地模拟
        }
    }
}