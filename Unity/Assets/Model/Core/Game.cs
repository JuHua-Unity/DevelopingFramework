namespace Model
{
    /// <summary>
    /// GameCore
    /// 框架核心
    /// </summary>
    public static class Game
    {
        public static Hotfix Hotfix { get; private set; } = null;

        public static void Start(byte[] assBytes, byte[] pdbBytes)
        {
            Hotfix = new Hotfix();
            Hotfix.InitHotfixAssembly(assBytes, pdbBytes);
            Hotfix.GotoHotfix();
        }

        public static void Close()
        {
            if (Hotfix == null)
            {
                return;
            }

            Hotfix.OnApplicationQuit?.Invoke();
        }

        public static void ReStart(byte[] assBytes, byte[] pdbBytes)
        {
            //关闭热更层
            Close();
            //GC回收
            System.GC.Collect();
            //重新启动热更层
            Start(assBytes, pdbBytes);
        }
    }
}