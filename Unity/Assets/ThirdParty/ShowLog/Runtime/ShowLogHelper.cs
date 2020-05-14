using UnityEngine;

namespace ShowLog
{
    internal static class ShowLogHelper
    {
        public static void Open()
        {
            if (LogGUI.Instance != null)
            {
                return;
            }

            var go = new GameObject("ShowLog");
            go.AddComponent<LogGUI>();
        }
    }
}