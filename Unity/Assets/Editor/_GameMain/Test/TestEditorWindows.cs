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

        private void Awake()
        {
        }

        private void OnGUI()
        {
            fold = EditorGUILayout.Foldout(fold, "Foldout");
            if (fold)
            {
                EditorGUILayout.LabelField("aaaaaaaaaaaaaaa");
            }
            EditorGUILayout.TextField("1", "2");

            if (GUILayout.Button("Save"))
            {
            }
        }
    }
}