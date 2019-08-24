namespace Game
{
    /// <summary>
    /// 日志类
    /// </summary>
    internal static class Log
    {
        [System.Diagnostics.Conditional("MODELLOG")]
        public static void Debug(object message)
        {
            UnityEngine.Debug.Log(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("MODELLOG")]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("MODELLOG")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("MODELLOG")]
        public static void LogException(System.Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [System.Diagnostics.Conditional("MODELLOG")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(JsonHelper.ToJson(message));
        }
    }
}