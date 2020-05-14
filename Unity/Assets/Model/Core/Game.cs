using System;
using ILRuntime.Hotfix;

namespace Model
{
    /// <summary>
    /// GameCore
    /// 框架核心
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// 启动流程
        /// 3:需要关闭 -> 2
        /// 2:需要GC -> 1
        /// 1:启动 -> 0
        /// 0:空闲
        /// 4:运行中
        /// </summary>
        internal static int StartProcess { get; set; }

        public static Hotfix Hotfix { get; private set; }

        public static void Start()
        {
            if (StartProcess != 0)
            {
                throw new Exception($"当前游戏启动流程状态={StartProcess}，不可启动！");
            }

            StartProcess = 1;
        }

        public static void ReStart()
        {
            if (StartProcess != 4)
            {
                throw new Exception($"当前游戏启动流程状态={StartProcess}，不可重启！");
            }

            StartProcess = 3;
        }

        internal static void Start(byte[] assBytes, byte[] pdbBytes)
        {
            if (StartProcess != -1)
            {
                throw new Exception($"当前游戏启动流程状态={StartProcess}，不可启动！");
            }

            if (Hotfix != null)
            {
                throw new Exception("当前Hotfix不为空，却要Start，请先Close Hotfix");
            }

            Hotfix = new Hotfix();
#if ILRuntime
            Log.Debug("当前使用的是ILRuntime模式！");
#else
            Log.Debug("当前使用的是Mono模式！");
#endif
            Hotfix.InitHotfixAssembly(assBytes, pdbBytes);
#if ILRuntime
            ILHelper.InitILRuntime(Hotfix.AppDomain);
#endif
            Hotfix.GotoHotfix();
        }

        internal static void Close()
        {
            Hotfix?.OnApplicationQuit?.Invoke();
            Hotfix = null;
        }

        internal static void LateUpdate()
        {
            Hotfix?.LateUpdate?.Invoke();
        }

        internal static void OnApplicationQuit()
        {
            Close();
        }

        internal static void OnApplicationFocus(bool focus)
        {
            Hotfix?.OnApplicationFocus?.Invoke(focus);
        }

        internal static void OnApplicationPause(bool pause)
        {
            Hotfix?.OnApplicationPause?.Invoke(pause);
        }
    }
}