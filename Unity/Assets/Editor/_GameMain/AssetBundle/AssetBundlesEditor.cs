using Hotfix;
using Model;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class AssetBundlesEditor : EditorWindow
    {
        public static void Open()
        {
            GetWindow<AssetBundlesEditor>().Show();
        }

        private const string absConfigPath = "Assets/Editor/_GameMain/Configs/AssetBundlesConfig.json";
        private const string buildsConfigPath = "Assets/Editor/_GameMain/Configs/AssetBundleBuildsConfig.json";

        private List<ABInfo> abs = new List<ABInfo>();
        private List<string> allABNames = null;

        private List<BuildInfo> builds = null;
        private List<string> builds_ShowName = null;
        private int builds_SelectIndex = 0;
        private int builds_SelectIndex_T = -1;
        private int build_SelectIndex = 0;
        private readonly string[] build_CopyToLocalType = new string[3] { "复制配置的那些", "不复制", "全部复制" };
        private BuildInfo build = new BuildInfo();
        private bool builds_ShowNewBtn = false;

        private Vector2 scrollPosition;
        private bool absGroup = false;
        private bool absCopyToLocal = false;
        private bool buildsInfo = false;

        private void Awake()
        {
            allABNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
            ReadABInfo();
            ReadBuildInfo();
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            ShowABInfos();
            EditorGUILayout.Space();
            ShowBuildInfos();
            EditorGUILayout.EndScrollView();
        }

        private void ShowABInfos()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("配置AssetBundle：", GUILayout.Width(120));
            if (GUILayout.Button("保存配置", GUILayout.Width(60)))
            {
                SaveABInfo();
            }

            EditorGUILayout.EndHorizontal();

            ShowABInfos_Group();
            ShowABInfos_CopyToLocal();
        }

        private void ShowABInfos_Group()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            Tab();
            absGroup = EditorGUILayout.ToggleLeft("配置AssetBundle所属分组", absGroup);
            EditorGUILayout.EndHorizontal();
            if (!absGroup)
            {
                return;
            }

            for (int i = 0; i < abs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Tab();
                EditorGUILayout.LabelField("AB:", GUILayout.Width(25));
                EditorGUILayout.LabelField(abs[i].Name.PadRight(200, '-'), GUILayout.Width(200));
                EditorGUILayout.LabelField("所属分组：", GUILayout.Width(60));
                abs[i].Group = EditorGUILayout.TextField(abs[i].Group);

                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowABInfos_CopyToLocal()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            Tab();
            absCopyToLocal = EditorGUILayout.ToggleLeft("配置AssetBundle在打包的时候是否复制到本地", absCopyToLocal);
            EditorGUILayout.EndHorizontal();
            if (!absCopyToLocal)
            {
                return;
            }

            for (int i = 0; i < abs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Tab();
                EditorGUILayout.LabelField("AB:", GUILayout.Width(25));
                EditorGUILayout.LabelField(abs[i].Name.PadRight(200, '-'), GUILayout.Width(200));
                EditorGUILayout.LabelField("复制到本地：", GUILayout.Width(70));
                abs[i].CopyToLocal = EditorGUILayout.Toggle(abs[i].CopyToLocal);

                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowBuildInfos()
        {
            buildsInfo = EditorGUILayout.BeginToggleGroup("选择或创建打包配置", buildsInfo);
            if (!buildsInfo)
            {
                EditorGUILayout.EndToggleGroup();
                return;
            }

            builds_SelectIndex = EditorGUILayout.Popup("已有配置：", builds_SelectIndex, builds_ShowName.ToArray());
            if (builds_SelectIndex < builds.Count)
            {
                if (builds_SelectIndex_T != builds_SelectIndex)
                {
                    builds_SelectIndex_T = builds_SelectIndex;
                    build.Clone(builds[builds_SelectIndex]);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            build.OutPath = EditorGUILayout.TextField("OutPath:", build.OutPath);
            if (GUILayout.Button("Browser", GUILayout.Width(80)))
            {
                string str = OpenFloder("../Release/");
                if (!string.IsNullOrEmpty(str))
                {
                    str = Path.Combine(str, Define.ABsPathParent);
                }

                build.OutPath = str;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            build.CopyToLocalPath = EditorGUILayout.TextField("CopyToLocalPath:", build.CopyToLocalPath);
            if (GUILayout.Button("Browser", GUILayout.Width(80)))
            {
                build.CopyToLocalPath = OpenFloder("Assets/StreamingAssets/AssetBundles/");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            build.CopyToServerPath = EditorGUILayout.TextField("CopyToServerPath:", build.CopyToServerPath);
            if (GUILayout.Button("Browser", GUILayout.Width(80)))
            {
                build.CopyToServerPath = OpenFloder("../Release/");
            }

            EditorGUILayout.EndHorizontal();

            build.Options = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Options:", build.Options);
            build.Target = (BuildTarget)EditorGUILayout.EnumPopup("Target:", build.Target);

            EditorGUILayout.BeginHorizontal();
            if (builds_SelectIndex < builds.Count)
            {
                if (GUILayout.Button("保存至当前选择的配置"))
                {
                    builds[builds_SelectIndex].Clone(build);
                    builds_ShowName[builds_SelectIndex] = build.ShowName;
                }

                if (GUILayout.Button("删除当前选择的配置"))
                {
                    builds.RemoveAt(builds_SelectIndex);
                    builds_ShowName.RemoveAt(builds_SelectIndex);
                }
            }

            builds_ShowNewBtn = true;
            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].Target == build.Target)
                {
                    builds_ShowNewBtn = false;
                }
            }

            if (builds_ShowNewBtn)
            {
                if (GUILayout.Button("另存为一个新的配置"))
                {
                    BuildInfo a = new BuildInfo();
                    a.Clone(build);
                    builds.Add(a);
                    builds_ShowName.Add(a.ShowName);
                    builds_SelectIndex = builds.Count - 1;
                }
            }

            if (GUILayout.Button("保存所有配置至本地"))
            {
                SaveBuildInfo();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Build"))
            {
                Build();
            }

            build_SelectIndex = EditorGUILayout.Popup(build_SelectIndex, build_CopyToLocalType, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndToggleGroup();
        }

        private void Build()
        {
            if (build == null)
            {
                return;
            }

            if (!Directory.Exists(build.OutPath))
            {
                Directory.CreateDirectory(build.OutPath);
            }

            AssetDatabase.Refresh();
            BuildPipeline.BuildAssetBundles(build.OutPath, build.Options, build.Target);
            AssetDatabase.Refresh();
            CopyABToLocal();
            AssetDatabase.Refresh();
            CopyABToServer();
            AssetDatabase.Refresh();
            Debug.Log($"打包结束！");
        }

        private void CopyABToServer()
        {
            ResVersion resVersion = new ResVersion { Res = new Dictionary<string, List<ResVersion.ResVersionInfo>>() };
            if (!resVersion.Res.ContainsKey(Define.ABsMainGroup))
            {
                resVersion.Res.Add(Define.ABsMainGroup, new List<ResVersion.ResVersionInfo>());
            }

            FileInfo fileInfo1 = new FileInfo(Path.Combine(build.OutPath, Define.ABsPathParent));
            resVersion.Res[Define.ABsMainGroup].Add(new ResVersion.ResVersionInfo()
            {
                File = Define.ABsPathParent,
                Size = fileInfo1.Length,
                MD5 = MD5Helper.FileMD5(fileInfo1.FullName)
            });

            List<string> files = new List<string>() { Define.ABsPathParent };
            for (int i = 0; i < abs.Count; i++)
            {
                files.Add(abs[i].Name);
                if (!resVersion.Res.ContainsKey(abs[i].Group))
                {
                    resVersion.Res.Add(abs[i].Group, new List<ResVersion.ResVersionInfo>());
                }

                FileInfo fileInfo = new FileInfo(Path.Combine(build.OutPath, abs[i].Name));
                resVersion.Res[abs[i].Group].Add(new ResVersion.ResVersionInfo()
                {
                    File = abs[i].Name,
                    Size = fileInfo.Length,
                    MD5 = MD5Helper.FileMD5(fileInfo.FullName)
                });
            }

            CopyTo(build.OutPath, build.CopyToServerPath, files);
            GenerateVersion(resVersion, Path.Combine(build.CopyToServerPath, "ResVersion.json"));
        }

        private void CopyABToLocal()
        {
            if (build_SelectIndex == 1)
            {
                return;
            }

            ResVersion resVersion = new ResVersion { Res = new Dictionary<string, List<ResVersion.ResVersionInfo>>() };
            if (!resVersion.Res.ContainsKey(Define.ABsMainGroup))
            {
                resVersion.Res.Add(Define.ABsMainGroup, new List<ResVersion.ResVersionInfo>());
            }

            FileInfo fileInfo1 = new FileInfo(Path.Combine(build.OutPath, Define.ABsPathParent));
            resVersion.Res[Define.ABsMainGroup].Add(new ResVersion.ResVersionInfo()
            {
                File = Define.ABsPathParent,
                Size = fileInfo1.Length,
                MD5 = MD5Helper.FileMD5(fileInfo1.FullName)
            });

            List<string> files = new List<string> { Define.ABsPathParent };
            for (int i = 0; i < abs.Count; i++)
            {
                if (abs[i].CopyToLocal || build_SelectIndex == 2)
                {
                    files.Add(abs[i].Name);
                    if (!resVersion.Res.ContainsKey(abs[i].Group))
                    {
                        resVersion.Res.Add(abs[i].Group, new List<ResVersion.ResVersionInfo>());
                    }

                    FileInfo fileInfo = new FileInfo(Path.Combine(build.OutPath, abs[i].Name));
                    resVersion.Res[abs[i].Group].Add(new ResVersion.ResVersionInfo()
                    {
                        File = abs[i].Name,
                        Size = fileInfo.Length,
                        MD5 = MD5Helper.FileMD5(fileInfo.FullName)
                    });
                }
            }

            CopyTo(build.OutPath, build.CopyToLocalPath, files);
            GenerateVersion(resVersion, Path.Combine(build.CopyToLocalPath, "ResVersion.json"));
        }

        private void CopyTo(string fromPath, string toPath, List<string> files)
        {
            if (Directory.Exists(toPath))
            {
                Directory.Delete(toPath, true);
            }

            Directory.CreateDirectory(toPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(fromPath);
            for (int i = 0; i < files.Count; i++)
            {
                var p1 = Path.Combine(directoryInfo.FullName, files[i]);
                var p2 = Path.Combine(toPath, files[i]);
                File.Copy(p1, p2);
            }

            Debug.Log($"复制{fromPath}到{toPath}完成!");
        }

        private void GenerateVersion(ResVersion resVersion, string path)
        {
            IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(resVersion), path);
            Debug.Log($"生成{path}完成!");
        }

        private void Tab(int num = 1)
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20 * num));
        }

        private string OpenFloder(string path = "")
        {
            return EditorUtility.OpenFolderPanel("选择路径文件夹：", path, "");
        }

        private void ReadABInfo()
        {
            var a = new List<ABInfo>();
            if (File.Exists(absConfigPath))
            {
                var b = AssetDatabase.LoadAssetAtPath<TextAsset>(absConfigPath);
                if (b != null && !string.IsNullOrEmpty(b.text))
                {
                    a = LitJson.JsonMapper.ToObject<List<ABInfo>>(b.text);
                }
            }

            for (int i = 0; i < allABNames.Count; i++)
            {
                string abName = allABNames[i];
                var item = new ABInfo() { Name = abName, CopyToLocal = false, Group = Define.ABsMainGroup };
                for (int j = 0; j < a.Count; j++)
                {
                    if (a[j].Name.Equals(abName))
                    {
                        item.CopyToLocal = a[j].CopyToLocal;
                        item.Group = a[j].Group;
                        break;
                    }
                }

                abs.Add(item);
            }
        }

        private void SaveABInfo()
        {
            IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(abs), absConfigPath);
            AssetDatabase.Refresh();
            Debug.Log($"保存{absConfigPath}完成!");
        }

        private void ReadBuildInfo()
        {
            if (File.Exists(buildsConfigPath))
            {
                var b = AssetDatabase.LoadAssetAtPath<TextAsset>(buildsConfigPath);
                if (b != null && !string.IsNullOrEmpty(b.text))
                {
                    builds = LitJson.JsonMapper.ToObject<List<BuildInfo>>(b.text);
                    builds_ShowName = new List<string>();
                    for (int i = 0; i < builds.Count; i++)
                    {
                        builds_ShowName.Add(builds[i].ShowName);
                    }

                    return;
                }
            }

            builds = new List<BuildInfo>();
            builds_ShowName = new List<string>();
        }

        private void SaveBuildInfo()
        {
            IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(builds), buildsConfigPath);
            AssetDatabase.Refresh();
            Debug.Log($"保存{buildsConfigPath}完成!");
        }

        private class BuildInfo
        {
            public string OutPath { get; set; }
            public string CopyToLocalPath { get; set; }
            public string CopyToServerPath { get; set; }
#if UNITY_STANDALONE
            public BuildAssetBundleOptions Options { get; set; } = BuildAssetBundleOptions.DeterministicAssetBundle;
#else
            public BuildAssetBundleOptions Options { get; set; } = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
#endif
            public BuildTarget Target { get; set; }

            public string ShowName
            {
                get
                {
                    return $"{Target}-{Options}";
                }
            }

            public void Clone(BuildInfo build)
            {
                OutPath = build.OutPath;
                CopyToLocalPath = build.CopyToLocalPath;
                CopyToServerPath = build.CopyToServerPath;
                Options = build.Options;
                Target = build.Target;
            }
        }

        private class ABInfo
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public bool CopyToLocal { get; set; }
        }
    }
}
