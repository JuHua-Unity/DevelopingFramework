using UnityEngine;

namespace ETHotfix
{
    public class GUITest : MonoBehaviour
    {
        private void Awake()
        {
        }

        private void OnGUI()
        {
            GUILayout.Window(0, new Rect(0, 0, Screen.width, Screen.height), SSS, "");
        }

        private void SSS(int id)
        {
            GUILayout.BeginScrollView(new Vector2(100, 100));
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.Label("11111111111111111111");
            GUILayout.EndScrollView();
        }
    }
}