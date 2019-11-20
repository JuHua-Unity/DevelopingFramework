namespace Model
{
    /// <summary>
    /// GameCore
    /// 框架核心
    /// </summary>
    public static class Game
    {
        public static DD DD;
        /// <summary>
        /// 启动流程
        /// 3:需要关闭 -> 2
        /// 2:需要GC -> 1
        /// 1:启动 -> 0
        /// 0:空闲
        /// </summary>
        internal static int StartProcess { get; set; } = 0;

        public static Hotfix Hotfix { get; private set; } = null;

        internal static void Start(byte[] assBytes, byte[] pdbBytes)
        {
            Hotfix = new Hotfix();
            Hotfix.InitHotfixAssembly(assBytes, pdbBytes);
            Hotfix.GotoHotfix();
        }

        internal static void Close()
        {
            if (Hotfix == null)
            {
                return;
            }

            Hotfix.OnApplicationQuit?.Invoke();
            Hotfix = null;
        }

        public static void ReStart()
        {
            StartProcess = 3;
        }
    }
}