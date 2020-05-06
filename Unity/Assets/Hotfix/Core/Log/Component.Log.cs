using System;

namespace Hotfix
{
    internal partial class Component
    {
        protected static void Log(object message)
        {
            Hotfix.Log.Debug(message);
        }

        protected static void Assertion(object message)
        {
            Hotfix.Log.Assertion(message);
        }

        protected static void Error(object message)
        {
            Hotfix.Log.Error(message);
        }

        protected static void Exception(Exception exception)
        {
            Hotfix.Log.Exception(exception);
        }

        protected static void Warning(object message)
        {
            Hotfix.Log.Warning(message);
        }
    }
}