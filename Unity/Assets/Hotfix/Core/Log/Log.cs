﻿namespace Hotfix
{
    /// <summary>
    /// 日志类
    /// </summary>
    internal static class Log
    {
        [System.Diagnostics.Conditional("HOTFIXLOG")]
        public static void Debug(object message)
        {
            UnityEngine.Debug.Log(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("HOTFIXLOG")]
        public static void Assertion(object message)
        {
            UnityEngine.Debug.LogAssertion(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("HOTFIXLOG")]
        public static void Error(object message)
        {
            UnityEngine.Debug.LogError(JsonHelper.ToJson(message));
        }

        [System.Diagnostics.Conditional("HOTFIXLOG")]
        public static void Exception(System.Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [System.Diagnostics.Conditional("HOTFIXLOG")]
        public static void Warning(object message)
        {
            UnityEngine.Debug.LogWarning(JsonHelper.ToJson(message));
        }
    }
}