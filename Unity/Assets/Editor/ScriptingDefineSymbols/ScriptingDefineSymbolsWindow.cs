using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    /// <summary>
    /// 宏定义窗口
    /// </summary>
    internal class ScriptingDefineSymbolsWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<ScriptingDefineSymbolsWindow>().Show();
        }

        private string define;
        private readonly List<string> defines = new List<string>();
        private readonly List<DefineInfo> defaultDefines = new List<DefineInfo>();

        private void Awake()
        {
            this.defaultDefines.Clear();

            this.defaultDefines.Add(new DefineInfo {Define = "DEFINE_SHOWLOG", Description = "打包时会影响初始场景的选择，有log会使用init_debug场景否则init场景", NeedBuild = true});
            this.defaultDefines.Add(new DefineInfo {Define = "DEFINE_MODELLOG", Description = "Model层的Log显示 有则显示", NeedBuild = true});
            this.defaultDefines.Add(new DefineInfo {Define = "DEFINE_HOTFIXLOG", Description = "Hotfix层的Log显示 有则显示", NeedBuild = false});
            this.defaultDefines.Add(new DefineInfo {Define = "DEFINE_HOTFIXEDITOR", Description = "Hotfix层的Editor模式 有则处于editor模式 发布时需要去掉此宏", NeedBuild = false});
            this.defaultDefines.Add(new DefineInfo {Define = "DEFINE_LOCALRES", Description = "资源模式 有则是本地资源模式 不需要打包AB", NeedBuild = false});
            this.defaultDefines.Add(new DefineInfo {Define = "ObjectView", Description = "组件的信息展示 只在editor模式有效", NeedBuild = false});
            this.defaultDefines.Add(new DefineInfo {Define = "ILRuntime", Description = "热更新模式 打包必须勾选", NeedBuild = true});
            this.defaultDefines.Add(new DefineInfo {Define = "ILRuntime_Pdb", Description = "热更新模式下 有则加载pdb 内存会更大 正式包是不需要这个的", NeedBuild = true});

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
                var list = new List<string>();
                for (var i = 0; i < this.defines.Count; i++)
                {
                    var a = this.defines[i];
                    a = a.Trim();
                    if (string.IsNullOrEmpty(a))
                    {
                        continue;
                    }

                    if (list.Contains(a))
                    {
                        continue;
                    }

                    list.Add(a);
                }

                this.define = string.Join(";", list);
                Debug.Log($"新宏定义：{this.define}");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(Define.TargetGroup, this.define);
                AssetDatabase.Refresh();
            }
        }

        private void SetList()
        {
            this.define = PlayerSettings.GetScriptingDefineSymbolsForGroup(Define.TargetGroup);
            this.defines.Clear();
            this.defines.AddRange(this.define.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries));
        }

        private void ShowDefault()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("默认的一些宏定义：");
            for (var i = 0; i < this.defaultDefines.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (this.defines.Contains(this.defaultDefines[i].Define))
                {
                    if (GUILayout.Button("移除", GUILayout.Width(80)))
                    {
                        this.defines.Remove(this.defaultDefines[i].Define);
                    }
                }
                else
                {
                    if (GUILayout.Button("添加", GUILayout.Width(80)))
                    {
                        this.defines.Add(this.defaultDefines[i].Define);
                    }
                }

                EditorGUILayout.LabelField(this.defaultDefines[i].Define, GUILayout.Width(200));
                EditorGUILayout.LabelField(this.defaultDefines[i].Description);
                EditorGUILayout.LabelField(this.defaultDefines[i].NeedBuild ? "会影响打包" : "不会影响打包", GUILayout.Width(100));

                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowDefines()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("当前的宏定义：");
            for (var i = 0; i < this.defines.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                this.defines[i] = EditorGUILayout.TextField(this.defines[i], GUILayout.Width(200));
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    this.defines.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                this.defines.Add("");
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