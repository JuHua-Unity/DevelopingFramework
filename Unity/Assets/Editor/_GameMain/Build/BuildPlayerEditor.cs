using Model;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    internal class BuildPlayerEditor : EditorWindow
    {
        public static void Open()
        {
            GetWindow<BuildPlayerEditor>().Show();
        }

        private const string path = "Assets/Editor/_GameMain/Configs/BuildPlayerConfig.json";
        private List<BuildPlayerIndo> builds;
        private BuildPlayerIndo build = new BuildPlayerIndo();
        private List<string> builds_Name;
        private int selectIndex = 0;
        private int selectIndex_T = -1;
        private bool builds_ShowNewBtn = false;

        private void Awake()
        {
            ReadInfos();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("选择或创建打包配置");

            selectIndex = EditorGUILayout.Popup("已有配置：", selectIndex, builds_Name.ToArray());
            if (selectIndex < builds.Count && selectIndex != selectIndex_T)
            {
                selectIndex_T = selectIndex;
                build.Clone(builds[selectIndex]);
            }

            EditorGUILayout.Space();
            build.Target = (BuildTarget)EditorGUILayout.EnumPopup("Target:", build.Target);

            EditorGUILayout.BeginHorizontal();
            build.OutPath = EditorGUILayout.TextField("OutPath:", build.OutPath);
            if (GUILayout.Button("Browser", GUILayout.Width(80)))
            {
                string str = EditorUtility.OpenFolderPanel("保存路径：", "../Release/", "");
                if (!string.IsNullOrEmpty(str))
                {
                    string name = "";
                    switch (build.Target)
                    {
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            name = "PC.exe";
                            break;
                        case BuildTarget.Android:
                            name = "Android.apk";
                            break;
                        default:
                            break;
                    }

                    str = Path.Combine(str, name);
                }

                build.OutPath = str;
            }
            EditorGUILayout.EndHorizontal();

            build.Development = EditorGUILayout.Toggle("Development:", build.Development);

            EditorGUILayout.BeginHorizontal();
            if (selectIndex < builds.Count)
            {
                if (GUILayout.Button("保存至当前选择的配置"))
                {
                    builds[selectIndex].Clone(build);
                    builds_Name[selectIndex] = build.ShowName;
                }

                if (GUILayout.Button("删除当前选择的配置"))
                {
                    builds.RemoveAt(selectIndex);
                    builds_Name.RemoveAt(selectIndex);
                }
            }

            builds_ShowNewBtn = true;
            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].ShowName == build.ShowName)
                {
                    builds_ShowNewBtn = false;
                }
            }

            if (builds_ShowNewBtn)
            {
                if (GUILayout.Button("另存为一个新的配置"))
                {
                    BuildPlayerIndo a = new BuildPlayerIndo();
                    a.Clone(build);
                    builds.Add(a);
                    builds_Name.Add(a.ShowName);
                    selectIndex = builds.Count - 1;
                }
            }

            if (GUILayout.Button("保存所有配置至本地"))
            {
                SaveInfos();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Build"))
            {
                Build();
            }
        }

        private void Build()
        {
            if (build == null || string.IsNullOrEmpty(build.OutPath))
            {
                return;
            }

#if DEFINE_SHOWLOG
            string[] levels = { "Assets/Scenes/Init_Debug.unity", };
#else
            string[] levels = { "Assets/Scenes/Init.unity", };
#endif
            BuildOptions options = BuildOptions.None;
            if (build.Development)
            {
                options = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
            }

            BuildPipeline.BuildPlayer(levels, build.OutPath, build.Target, options);
        }

        private void ReadInfos()
        {
            builds_Name = new List<string>();
            if (File.Exists(path))
            {
                var b = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                builds = LitJson.JsonMapper.ToObject<List<BuildPlayerIndo>>(b.text);
                for (int i = 0; i < builds.Count; i++)
                {
                    builds_Name.Add(builds[i].ShowName);
                }

                return;
            }

            builds = new List<BuildPlayerIndo>();
        }

        private void SaveInfos()
        {
            IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(builds), path);
            AssetDatabase.Refresh();
        }

        private class BuildPlayerIndo
        {
            public string OutPath { get; set; }
            public BuildTarget Target { get; set; }
            public bool Development { get; set; }

            public string ShowName
            {
                get
                {
                    if (Development)
                    {
                        return $"{Target}-Debug-{OutPath.Replace("/", "-")}";
                    }
                    else
                    {
                        return $"{Target}-Release-{OutPath.Replace("/", "-")}";
                    }
                }
            }

            public void Clone(BuildPlayerIndo buildPlayer)
            {
                OutPath = buildPlayer.OutPath;
                Target = buildPlayer.Target;
                Development = buildPlayer.Development;
            }
        }
    }
}