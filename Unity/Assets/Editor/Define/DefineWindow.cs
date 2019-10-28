using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    /// <summary>
    /// 宏定义窗口
    /// </summary>
    internal class DefineWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<DefineWindow>().Show();
        }

        private string define;
        private readonly List<string> defines = new List<string>();
        private readonly List<DefineInfo> defaultDefines = new List<DefineInfo>();

        private void Awake()
        {
            defaultDefines.Clear();

            defaultDefines.Add(new DefineInfo() { Define = "DEFINE_SHOWLOG", Description = "打包时会影响初始场景的选择，有log会使用init_debug场景否则init场景", NeedBuild = true });
            defaultDefines.Add(new DefineInfo() { Define = "DEFINE_MODELLOG", Description = "Model层的Log显示 有则显示", NeedBuild = true });
            defaultDefines.Add(new DefineInfo() { Define = "DEFINE_HOTFIXLOG", Description = "Hotfix层的Log显示 有则显示", NeedBuild = false });
            defaultDefines.Add(new DefineInfo() { Define = "DEFINE_LOCALRES", Description = "资源模式 有则是本地资源模式 不需要打包AB", NeedBuild = true });
            defaultDefines.Add(new DefineInfo() { Define = "ComponentView", Description = "组件的信息展示 只在editor模式有效", NeedBuild = false });
            defaultDefines.Add(new DefineInfo() { Define = "ILRuntime", Description = "热更新模式 打包必须勾选", NeedBuild = true });
            defaultDefines.Add(new DefineInfo() { Define = "ILRuntime_Pdb", Description = "热更新模式下 有则加载pdb 内存会更大 正式包是不需要这个的", NeedBuild = true });

            SetList();
        }

        private void OnGUI()
        {
            ShowDefault();
            ShowDefines();
            SaveDefines();
        }

        private void SaveDefines()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Save"))
            {
                List<string> list = new List<string>();
                for (int i = 0; i < defines.Count; i++)
                {
                    if (list.Contains(defines[i]))
                    {
                        continue;
                    }

                    list.Add(defines[i]);
                }
                define = string.Join(";", list);
                Debug.Log($"新宏定义：{define}");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(PlatformDefine.TargetGroup, define);
                AssetDatabase.Refresh();
            }
        }

        private void SetList()
        {
            define = PlayerSettings.GetScriptingDefineSymbolsForGroup(PlatformDefine.TargetGroup);
            defines.Clear();
            defines.AddRange(define.Split(new string[1] { ";" }, System.StringSplitOptions.RemoveEmptyEntries));
        }

        private void ShowDefault()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("默认的一些宏定义：");
            for (int i = 0; i < defaultDefines.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (defines.Contains(defaultDefines[i].Define))
                {
                    if (GUILayout.Button("移除", GUILayout.Width(80)))
                    {
                        defines.Remove(defaultDefines[i].Define);
                    }
                }
                else
                {
                    if (GUILayout.Button("添加", GUILayout.Width(80)))
                    {
                        defines.Add(defaultDefines[i].Define);
                    }
                }

                EditorGUILayout.LabelField(defaultDefines[i].Define, GUILayout.Width(200));
                EditorGUILayout.LabelField(defaultDefines[i].Description);
                EditorGUILayout.LabelField(defaultDefines[i].NeedBuild ? "会影响打包" : "不会影响打包", GUILayout.Width(100));

                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowDefines()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("当前的宏定义：");
            for (int i = 0; i < defines.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                defines[i] = EditorGUILayout.TextField(defines[i], GUILayout.Width(200));
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    defines.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                defines.Add("");
            }
        }

        private class DefineInfo
        {
            public string Define { get; set; }
            public string Description { get; set; }
            public bool NeedBuild { get; set; }
        }
    }
}