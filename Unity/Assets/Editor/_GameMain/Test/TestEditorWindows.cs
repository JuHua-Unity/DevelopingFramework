using Model;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class TestEditorWindows : EditorWindow
    {
        public static void Open()
        {
            GetWindow<TestEditorWindows>().Show();
        }

        private bool fold = false;
        private const string path = "Assets/_GameMain/Res/Configs/LaunchOptions.json";
        private LaunchOptions launchOptions;

        private void Awake()
        {
            launchOptions = IOHelper.StreamReader<LaunchOptions>(path);
        }

        private void OnGUI()
        {
            fold = EditorGUILayout.Foldout(fold, "Foldout");
            if (fold)
            {
                EditorGUILayout.LabelField("aaaaaaaaaaaaaaa");
            }
            EditorGUILayout.TextField("1", "2");

            ObjectDrawerHelper.Draw(launchOptions, false);

            if (GUILayout.Button("Save"))
            {
                IOHelper.StreamWriter(LitJson.JsonMapper.ToJson(launchOptions), path);
            }
        }
    }
}