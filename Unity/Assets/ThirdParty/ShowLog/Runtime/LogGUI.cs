using UnityEngine;

namespace ShowLog
{
    internal class LogGUI : MonoBehaviour
    {
        public static LogGUI Instance { get; private set; }

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            Application.logMessageReceived += GotLog;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            Application.logMessageReceived -= GotLog;
            Instance = null;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            ShowUI();
        }

        #region GUI显示

        private bool show;

        private void ShowUI()
        {
            if (this.show)
            {
                if (GUILayout.Button("Close"))
                {
                    this.show = false;
                }
            }
            else
            {
                if (GUILayout.Button("Show"))
                {
                    this.show = true;
                }
            }
        }

        #endregion

        #region 数据

        private static void GotLog(string condition, string stacktrace, LogType type)
        {
        }

        #endregion
    }
}