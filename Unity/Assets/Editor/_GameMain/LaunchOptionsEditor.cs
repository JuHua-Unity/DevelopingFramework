﻿using Model;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class LaunchOptionsEditor : EditorWindow
    {
        public static void Open()
        {
            GetWindow<LaunchOptionsEditor>().Show();
        }

        private const string path = "Assets/_GameMain/Res/Configs/LaunchOptions.json";
        private LaunchOptions launchOptions;

        private void Awake()
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            using (var sr = new StreamReader(path))
            {
                string str = sr.ReadToEnd();
                if (string.IsNullOrEmpty(str))
                {
                    launchOptions = new LaunchOptions();
                }
                else
                {
                    launchOptions = LitJson.JsonMapper.ToObject<LaunchOptions>(str);
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("代码预制体名字：");
            launchOptions.CodeABName = EditorGUILayout.TextField(launchOptions.CodeABName);

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
            {
                IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(launchOptions), path);
                AssetDatabase.Refresh();
            }
        }
    }
}