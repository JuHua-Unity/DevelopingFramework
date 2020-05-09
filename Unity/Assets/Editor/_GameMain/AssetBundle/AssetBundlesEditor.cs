using System.Collections.Generic;
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

        public static readonly string ABsConfigPath = $"{Define.EditorConfigsPath}AssetBundlesConfig.json";

        private readonly List<ABInfo> abs = new List<ABInfo>();
        private List<string> allABNames;

        private Vector2 scrollPosition;
        private readonly bool[] foldouts = {true, true, true};

        private void Awake()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            this.allABNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
            ReadABInfo();
        }

        private void OnGUI()
        {
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            ShowABInfos();
            EditorGUILayout.EndScrollView();
        }

        private void ShowABInfos()
        {
            this.foldouts[0] = EditorGUILayout.Foldout(this.foldouts[0], "配置AssetBundle", true);
            if (this.foldouts[0])
            {
                ShowABInfos_Group();
                ShowABInfos_CopyToLocal();

                EditorGUILayout.BeginHorizontal();
                Tab();
                if (GUILayout.Button("保存配置", GUILayout.Width(60)))
                {
                    SaveABInfo();
                }

                if (GUILayout.Button("保存配置", GUILayout.Width(60)))
                {
                    SaveABInfo();
                }

                EditorGUILayout.EndVertical();
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

        //private void CopyABToServer()
        //{
        //    ResVersion resVersion = new ResVersion { Res = new Dictionary<string, List<ResVersion.ResVersionInfo>>() };
        //    if (!resVersion.Res.ContainsKey(Define.ABsMainGroup))
        //    {
        //        resVersion.Res.Add(Define.ABsMainGroup, new List<ResVersion.ResVersionInfo>());
        //    }

        //    FileInfo fileInfo1 = new FileInfo(Path.Combine(build.OutPath, Define.ABsPathParent));
        //    resVersion.Res[Define.ABsMainGroup].Add(new ResVersion.ResVersionInfo()
        //    {
        //        File = Define.ABsPathParent,
        //        Size = fileInfo1.Length,
        //        MD5 = MD5Helper.FileMD5(fileInfo1.FullName)
        //    });

        //    List<string> files = new List<string>() { Define.ABsPathParent };
        //    for (int i = 0; i < abs.Count; i++)
        //    {
        //        files.Add(abs[i].Name);
        //        if (!resVersion.Res.ContainsKey(abs[i].Group))
        //        {
        //            resVersion.Res.Add(abs[i].Group, new List<ResVersion.ResVersionInfo>());
        //        }

        //        FileInfo fileInfo = new FileInfo(Path.Combine(build.OutPath, abs[i].Name));
        //        resVersion.Res[abs[i].Group].Add(new ResVersion.ResVersionInfo()
        //        {
        //            File = abs[i].Name,
        //            Size = fileInfo.Length,
        //            MD5 = MD5Helper.FileMD5(fileInfo.FullName)
        //        });
        //    }

        //    CopyTo(build.OutPath, build.CopyToServerPath, files);
        //    GenerateVersion(resVersion, Path.Combine(build.CopyToServerPath, "ResVersion.json"));
        //}

        //private void CopyABToLocal()
        //{
        //    if (build_SelectIndex == 1)
        //    {
        //        return;
        //    }

        //    ResVersion resVersion = new ResVersion { Res = new Dictionary<string, List<ResVersion.ResVersionInfo>>() };
        //    if (!resVersion.Res.ContainsKey(Define.ABsMainGroup))
        //    {
        //        resVersion.Res.Add(Define.ABsMainGroup, new List<ResVersion.ResVersionInfo>());
        //    }

        //    FileInfo fileInfo1 = new FileInfo(Path.Combine(build.OutPath, Define.ABsPathParent));
        //    resVersion.Res[Define.ABsMainGroup].Add(new ResVersion.ResVersionInfo()
        //    {
        //        File = Define.ABsPathParent,
        //        Size = fileInfo1.Length,
        //        MD5 = MD5Helper.FileMD5(fileInfo1.FullName)
        //    });

        //    List<string> files = new List<string> { Define.ABsPathParent };
        //    for (int i = 0; i < abs.Count; i++)
        //    {
        //        if (abs[i].CopyToLocal || build_SelectIndex == 2)
        //        {
        //            files.Add(abs[i].Name);
        //            if (!resVersion.Res.ContainsKey(abs[i].Group))
        //            {
        //                resVersion.Res.Add(abs[i].Group, new List<ResVersion.ResVersionInfo>());
        //            }

        //            FileInfo fileInfo = new FileInfo(Path.Combine(build.OutPath, abs[i].Name));
        //            resVersion.Res[abs[i].Group].Add(new ResVersion.ResVersionInfo()
        //            {
        //                File = abs[i].Name,
        //                Size = fileInfo.Length,
        //                MD5 = MD5Helper.FileMD5(fileInfo.FullName)
        //            });
        //        }
        //    }

        //    CopyTo(build.OutPath, build.CopyToLocalPath, files);
        //    GenerateVersion(resVersion, Path.Combine(build.CopyToLocalPath, "ResVersion.json"));
        //}

        //private void CopyTo(string fromPath, string toPath, List<string> files)
        //{
        //    if (Directory.Exists(toPath))
        //    {
        //        Directory.Delete(toPath, true);
        //    }

        //    Directory.CreateDirectory(toPath);
        //    DirectoryInfo directoryInfo = new DirectoryInfo(fromPath);
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        var p1 = Path.Combine(directoryInfo.FullName, files[i]);
        //        var p2 = Path.Combine(toPath, files[i]);
        //        File.Copy(p1, p2);
        //    }

        //    Debug.Log($"复制{fromPath}到{toPath}完成!");
        //}

        //private void GenerateVersion(ResVersion resVersion, string path)
        //{
        //    IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(resVersion), path);
        //    Debug.Log($"生成{path}完成!");
        //}

        private void Tab(int num = 1)
        {
            EditorGUILayout.LabelField("", GUILayout.Width(20 * num));
        }

        private void ReadABInfo()
        {
            var a = IOHelper.StreamReader<List<ABInfo>>(ABsConfigPath);
            for (var i = 0; i < this.allABNames.Count; i++)
            {
                var abName = this.allABNames[i];
                var item = new ABInfo {Name = abName};
                for (var j = 0; j < a.Count; j++)
                {
                    if (a[j].Name.Equals(abName))
                    {
                        item.CopyToLocal = a[j].CopyToLocal;
                        item.Group = a[j].Group;
                        break;
                    }
                }

                this.abs.Add(item);
            }
        }

        private void SaveABInfo()
        {
            IOHelper.StreamWriter(JsonMapper.ToJson(this.abs), ABsConfigPath);
            AssetDatabase.Refresh();
            Debug.Log($"保存{ABsConfigPath}完成!");
        }

        public static List<string> GetLocalABs()
        {
            var list = new List<string>();
            var a = IOHelper.StreamReader<List<ABInfo>>(ABsConfigPath);
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i].CopyToLocal)
                {
                    list.Add(a[i].Name);
                }
            }

            return list;
        }

        private class ABInfo
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