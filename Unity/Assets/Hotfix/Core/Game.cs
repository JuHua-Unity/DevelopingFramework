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
                if (componentRoot == null)
                {
                    Component.GameRoot = UnityEngine.GameObject.Find("/GameRoot");

#if UNITY_EDITOR && !ILRuntime && ComponentView

                    Component.ParentNullRoot = new UnityEngine.GameObject("DisposedComponentRoot");
                    Component.ParentNullRoot.transform.SetParent(Component.GameRoot.transform, false);
                    Component.ParentNullRoot.SetActive(false);

#endif

                    componentRoot = new ComponentRoot();
                }

                return componentRoot;
            }
        }

        public static void Close()
        {
            componentRoot?.Dispose();
            componentRoot = null;

            componentSystem?.Dispose();
            componentSystem = null;

            objectPool?.Dispose();
            objectPool = null;

            //最后执行

#if UNITY_EDITOR && !ILRuntime && ComponentView

            UnityEngine.Object.DestroyImmediate(Component.ParentNullRoot);
            Component.ParentNullRoot = null;
#endif

            Component.GameRoot = null;
        }
    }
}