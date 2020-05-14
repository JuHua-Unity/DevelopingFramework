using System.Collections.Generic;
using Hotfix;
using LitJson;
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

        public static List<ABInfo> GetABs()
        {
            var list = new List<ABInfo>();
            var allABNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
            var a = IOHelper.StreamReader<List<ABInfo>>(ABsConfigPath);
            for (var i = 0; i < allABNames.Count; i++)
            {
                var abName = allABNames[i];
                var item = new ABInfo {Name = abName};
                for (var j = 0; j < a.Count; j++)
                {
                    if (!a[j].Name.Equals(abName))
                    {
                        continue;
                    }

                    item.CopyToLocal = a[j].CopyToLocal;
                    item.Group = a[j].Group;
                    break;
                }

                list.Add(item);
            }

            return list;
        }

        private static readonly string ABsConfigPath = $"{Define.EditorConfigsPath}AssetBundlesConfig.json";
        private const string abGroupPath = "Assets/_GameMain/Res/Configs/AssetBundleGroup.json";

        private readonly List<ABInfo> abs = new List<ABInfo>();

        private Vector2 scrollPosition;
        private readonly bool[] foldouts = {true, true, true};

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            ReadABInfo();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            ShowABInfos();
            EditorGUILayout.EndScrollView();
        }

        private void ShowABInfos()
        {
            this.foldouts[0] = EditorGUILayout.Foldout(this.foldouts[0], "配置AssetBundle", true);
            if (!this.foldouts[0])
            {
                return;
            }

            ShowABInfos_Group();
            ShowABInfos_CopyToLocal();

            EditorGUILayout.Space();
            if (GUILayout.Button("保存配置", GUILayout.Width(60)))
            {
                SaveABInfo();
            }
        }

        private void ShowABInfos_Group()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            Tab();
            EditorGUILayout.BeginVertical();
            this.foldouts[1] = EditorGUILayout.Foldout(this.foldouts[1], "配置AssetBundle所属分组", true);
            if (this.foldouts[1])
            {
                for (var i = 0; i < this.abs.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    Tab();
                    EditorGUILayout.LabelField("AB:", GUILayout.Width(25));
                    EditorGUILayout.LabelField(this.abs[i].Name.PadRight(200, '-'), GUILayout.Width(200));
                    EditorGUILayout.LabelField("所属分组：", GUILayout.Width(60));
                    this.abs[i].Group = EditorGUILayout.TextField(this.abs[i].Group);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void ShowABInfos_CopyToLocal()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            Tab();

            EditorGUILayout.BeginVertical();
            this.foldouts[2] = EditorGUILayout.Foldout(this.foldouts[2], "配置AssetBundle在打包的时候是否复制到本地", true);
            if (this.foldouts[2])
            {
                for (var i = 0; i < this.abs.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    Tab();
                    EditorGUILayout.LabelField("AB:", GUILayout.Width(25));
                    EditorGUILayout.LabelField(this.abs[i].Name.PadRight(200, '-'), GUILayout.Width(200));
                    EditorGUILayout.LabelField("复制到本地：", GUILayout.Width(70));
                    this.abs[i].CopyToLocal = EditorGUILayout.Toggle(this.abs[i].CopyToLocal);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private static void Tab(int num = 1)
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20 * num));
        }

        private void ReadABInfo()
        {
            this.abs.Clear();
            this.abs.AddRange(GetABs());
        }

        private void SaveABInfo()
        {
            IOHelper.StreamWriter(JsonMapper.ToJson(this.abs), ABsConfigPath);
            AssetDatabase.Refresh();
            Debug.Log($"保存{ABsConfigPath}完成!");

            SaveABGroup();
        }

        private void SaveABGroup()
        {
            var a = new AssetBundleGroup();
            if (this.abs.Count > 0)
            {
                a.Dic = new Dictionary<string, List<string>>();
                for (var i = 0; i < this.abs.Count; i++)
                {
                    var b = this.abs[i];
                    var key = b.Group;
                    var value = b.Name;
                    if (a.Dic.ContainsKey(key))
                    {
                        a.Dic[key].Add(value);
                    }
                    else
                    {
                        a.Dic.Add(key, new List<string> {value});
                    }
                }
            }

            IOHelper.StreamWriter(JsonMapper.ToJson(a), abGroupPath);
            AssetDatabase.Refresh();
            Debug.Log($"保存{abGroupPath}完成!");
        }

        public class ABInfo
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public bool CopyToLocal { get; set; }

            public ABInfo()
            {
                this.Group = "Main";
                this.CopyToLocal = false;
            }
        }
    }
}