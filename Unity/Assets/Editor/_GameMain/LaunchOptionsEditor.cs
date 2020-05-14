using System.IO;
using LitJson;
using Model;
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

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            this.launchOptions = IOHelper.StreamReader<LaunchOptions>(path);
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("代码预制体名字：");
            this.launchOptions.CodeABName = EditorGUILayout.TextField(this.launchOptions.CodeABName);

            EditorGUILayout.EndHorizontal();

            if (!GUILayout.Button("Save"))
            {
                return;
            }

            IOHelper.StreamWriter(JsonMapper.ToJson(this.launchOptions), path);
            AssetDatabase.Refresh();
        }
    }
}