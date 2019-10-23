namespace Hotfix
{
    /// <summary>
    /// GameCore
    /// 框架核心
    /// </summary>
    internal static class Game
    {
        private static ObjectPool objectPool;
        private static ComponentSystem componentSystem;
        private static ComponentRoot componentRoot;

        /// <summary>
        /// 对象池
        /// </summary>
        public static ObjectPool ObjectPool
        {
            get
            {
                return objectPool ?? (objectPool = new ObjectPool());
            }
        }

        /// <summary>
        /// 组件的事件系统
        /// </summary>
        public static ComponentSystem ComponentSystem
        {
            get
            {
                return componentSystem ?? (componentSystem = new ComponentSystem());
            }
        }

        /// <summary>
        /// 组件的根
        /// </summary>
        public static ComponentRoot ComponentRoot
        {
            get
            {
                return componentRoot ?? (componentRoot = new ComponentRoot());
            }
        }

        public static void Close()
        {
            objectPool?.Dispose();
            objectPool = null;
            componentSystem?.Dispose();
            componentSystem = null;
        }
    }
}