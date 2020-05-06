using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private bool buildFold = true;
        private bool builderFold = true;
        private bool development;
        private string abVersion = "0.0.1";
        private string appVersion = "0.0.1";
        private LocalRes res = LocalRes.复制配置部分_打包版本;

        private void OnGUI()
        {
            EditorGUILayout.Space();
            Build();
            EditorGUILayout.Space();
            Builder();
        }

        private void Build()
        {
            this.buildFold = EditorGUILayout.Foldout(this.buildFold, "AssetBundle");
            if (!this.buildFold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            EditorGUILayout.BeginVertical();
            BuildGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void BuildGUI()
        {
            this.abVersion = EditorGUILayout.TextField("Version", this.abVersion);
            this.res = (LocalRes) EditorGUILayout.EnumPopup("资源复制：", this.res);

            if (GUILayout.Button("BuildAssetBundles"))
            {
                RunBuild();
            }
        }

        private void RunBuild()
        {
            var outPath = $"{Define.BuildPath}{Define.TargetName}/{Model.Define.ABsPathParent}";
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
            Debug.Log($"BuildAssetBundles complete");
            CopyABs(outPath);
            Debug.Log($"CopyAssetBundles complete");
            AssetDatabase.Refresh();
        }

        private void CopyABs(string outPath)
        {
            var localABs = GetLocalABs();
            var directory = new DirectoryInfo(outPath);
            var files = directory.GetFiles().Where(f => !f.Extension.Equals(".manifest")) as List<FileInfo>;
            var allABs = files.Select(t => t.Name).ToList();

            if (localABs == null)
            {
                localABs = files.Select(t => t.Name).ToList();
            }

            var serverPath = GetABServerPath();
            Debug.Log($"Server:{serverPath}");
            var localPath = GetABLocalPath();
            Debug.Log($"Local:{localPath}");

            for (var i = 0; i < files.Count; i++)
            {
                var fileName = files[i].Name;
                if (localABs.Contains(fileName))
                {
                    files[i].CopyTo(Path.Combine(localPath, fileName));
                }

                if (allABs.Contains(fileName))
                {
                    files[i].CopyTo(Path.Combine(serverPath, fileName));
                }
            }
        }

        private List<string> GetLocalABs()
        {
            List<string> list = null;

            switch (this.res)
            {
                case LocalRes.不复制_纯热更:
                    list = new List<string>();
                    break;
                case LocalRes.复制配置部分_打包版本:
                    list = new List<string>(AssetBundlesEditor.GetLocalABs());
                    break;
            }

            return list;
        }

        private string GetABServerPath()
        {
            if (string.IsNullOrEmpty(this.abVersion))
            {
                Debug.LogError($"abversion is empty!");
                return null;
            }

            string path;
            switch (Define.Target)
            {
                case BuildTarget.Android:
                    path = $"../Release/Server/Android/{this.abVersion}";
                    break;
                case BuildTarget.StandaloneWindows64:
                    path = $"../Release/Server/Standalone/{this.abVersion}";
                    break;
                case BuildTarget.iOS:
                    path = $"../Release/Server/IOS/{this.abVersion}";
                    break;
                default:
                    Debug.LogError($"不支持[{Define.Target}]打包！");
                    return null;
            }

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

        private void Builder()
        {
            this.builderFold = EditorGUILayout.Foldout(this.builderFold, "Build");
            if (!this.builderFold)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            EditorGUILayout.BeginVertical();
            BuilderM();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void BuilderM()
        {
            this.appVersion = EditorGUILayout.TextField("Version", this.appVersion);
            this.development = EditorGUILayout.ToggleLeft("Development", this.development);

            if (GUILayout.Button("BuildPlayer"))
            {
                RunBuilder();
            }
        }

        private void RunBuilder()
        {
#if DEFINE_SHOWLOG
            string[] levels = { "Assets/Scenes/Init_Debug.unity", };
#else
            string[] levels = {"Assets/Scenes/Init.unity",};
#endif
            var version = this.appVersion;
            if (string.IsNullOrEmpty(version))
            {
                Debug.LogError($"version is empty!");
                return;
            }

            PlayerSettings.bundleVersion = version;
            string outPath;
            switch (Define.Target)
            {
                case BuildTarget.Android:
                    outPath = $"../Release/Build/Android/";
                    if (!Directory.Exists(outPath))
                    {
                        Directory.CreateDirectory(outPath);
                    }

                    outPath = $"{outPath}Game-{version}.apk";
                    if (File.Exists(outPath))
                    {
                        Debug.Log($"{outPath} 已删除！");
                        File.Delete(outPath);
                    }

                    break;
                case BuildTarget.StandaloneWindows64:
                    outPath = $"../Release/Build/Standalone/{version}/";
                    if (!Directory.Exists(outPath))
                    {
                        Debug.Log($"{outPath} 已删除！");
                        Directory.Delete(outPath, true);
                    }

                    Directory.CreateDirectory(outPath);
                    outPath = $"{outPath}Game.exe";
                    break;
                case BuildTarget.iOS:
                    outPath = $"../Release/Build/IOS/{version}/";
                    if (Directory.Exists(outPath))
                    {
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
            Debug.Log($"BuildPlayer complete");
        }

        private enum LocalRes
        {
            不复制_纯热更,
            复制配置部分_打包版本,
            复制全部_本地模拟
        }
    }
}