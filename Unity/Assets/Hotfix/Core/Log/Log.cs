using System;
using System.Diagnostics;

namespace Hotfix
{
    /// <summary>
    /// 日志类
    /// </summary>
    internal static class Log
    {
        [Conditional("DEFINE_HOTFIXLOG")]
        public static void Debug(object message)
        {
            UnityEngine.Debug.Log(JsonHelper.ToJson(message));
        }

        [Conditional("DEFINE_HOTFIXLOG")]
        public static void Assertion(object message)
        {
            UnityEngine.Debug.LogAssertion(JsonHelper.ToJson(message));
        }

        [Conditional("DEFINE_HOTFIXLOG")]
        public static void Error(object message)
        {
            UnityEngine.Debug.LogError(JsonHelper.ToJson(message));
        }

        [Conditional("DEFINE_HOTFIXLOG")]
        public static void Exception(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [Conditional("DEFINE_HOTFIXLOG")]
        public static void Warning(object message)
        {
            UnityEngine.Debug.LogWarning(JsonHelper.ToJson(message));
        }
    }
}