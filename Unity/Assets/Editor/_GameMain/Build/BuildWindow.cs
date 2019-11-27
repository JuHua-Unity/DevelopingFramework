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
        private bool development = false;
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
            buildFold = EditorGUILayout.Foldout(buildFold, "AssetBundle");
            if (!buildFold)
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
            abVersion = EditorGUILayout.TextField("Version", abVersion);
            res = (LocalRes)EditorGUILayout.EnumPopup("资源复制：", res);

            if (GUILayout.Button("BuildAssetBundles"))
            {
                RunBuild();
            }
        }

        private void RunBuild()
        {
            var outPath = "../Release/Error";
            switch (PlatformDefine.Target)
            {
                case BuildTarget.Android:
                    outPath = $"../Release/Android/{Model.Define.ABsPathParent}";
                    break;
                case BuildTarget.StandaloneWindows64:
                    outPath = $"../Release/Standalone/{Model.Define.ABsPathParent}";
                    break;
                case BuildTarget.iOS:
                    outPath = $"../Release/IOS/{Model.Define.ABsPathParent}";
                    break;
                default:
                    Debug.LogError($"不支持[{PlatformDefine.Target}]打包！");
                    return;
            }

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            var options = BuildAssetBundleOptions.None;
            switch (PlatformDefine.Target)
            {
                case BuildTarget.StandaloneWindows64:
                    options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.StrictMode;
                    break;
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.StrictMode | BuildAssetBundleOptions.ChunkBasedCompression;
                    break;
                default:
                    Debug.LogError($"不支持[{PlatformDefine.Target}]打包！");
                    return;
            }

            var target = PlatformDefine.Target;
            BuildPipeline.BuildAssetBundles(outPath, options, target);
            AssetDatabase.Refresh();
            Debug.Log($"BuildAssetBundles complete");
            CopyABs(outPath);
            Debug.Log($"CopyAssetBundles complete");
            AssetDatabase.Refresh();
        }

        private void CopyABs(string outPath)
        {
            List<string> allABs = new List<string>();
            List<string> localABs = GetLocalABs();
            DirectoryInfo directory = new DirectoryInfo(outPath);
            var files = directory.GetFiles().Where((f) => { return !f.Extension.Equals(".manifest"); }) as List<FileInfo>;
            for (int i = 0; i < files.Count; i++)
            {
                allABs.Add(files[i].Name);
            }

            if (localABs == null)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    localABs.Add(files[i].Name);
                }
            }

            string serverPath = GetABServerPath();
            Debug.Log($"Server:{serverPath}");
            string localPath = GetABLocalPath();
            Debug.Log($"Local:{localPath}");

            for (int i = 0; i < files.Count; i++)
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

            switch (res)
            {
                case LocalRes.不复制_纯热更:
                    list = new List<string>();
                    break;
                case LocalRes.复制配置部分_打包版本:
                    list = new List<string>(AssetBundlesEditor.GetLocalABs());
                    break;
                default:
                    break;
            }

            return list;
        }

        private string GetABServerPath()
        {
            if (string.IsNullOrEmpty(abVersion))
            {
                Debug.LogError($"abversion is empty!");
                return null;
            }

            var path = "../Release/Server/Error";
            switch (PlatformDefine.Target)
            {
                case BuildTarget.Android:
                    path = $"../Release/Server/Android/{abVersion}";
                    break;
                case BuildTarget.StandaloneWindows64:
                    path = $"../Release/Server/Standalone/{abVersion}";
                    break;
                case BuildTarget.iOS:
                    path = $"../Release/Server/IOS/{abVersion}";
                    break;
                default:
                    Debug.LogError($"不支持[{PlatformDefine.Target}]打包！");
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
            string path = $"{Application.streamingAssetsPath}/{Model.Define.ABsMainGroup}";
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
            builderFold = EditorGUILayout.Foldout(builderFold, "Build");
            if (!builderFold)
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
            appVersion = EditorGUILayout.TextField("Version", appVersion);
            development = EditorGUILayout.ToggleLeft("Development", development);

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
            string[] levels = { "Assets/Scenes/Init.unity", };
#endif
            var version = appVersion;
            if (string.IsNullOrEmpty(version))
            {
                Debug.LogError($"version is empty!");
                return;
            }

            PlayerSettings.bundleVersion = version;
            var outPath = "../Release/Build/Error";
            switch (PlatformDefine.Target)
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
                    Debug.LogError($"不支持[{PlatformDefine.Target}]打包！");
                    return;
            }

            BuildOptions options = BuildOptions.None;
            if (development)
            {
                options = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
            }

            var target = PlatformDefine.Target;
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