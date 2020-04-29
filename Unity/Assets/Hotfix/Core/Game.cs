using UnityEngine;

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
        public static ObjectPool ObjectPool => objectPool ?? (objectPool = new ObjectPool());

        /// <summary>
        /// 组件的事件系统
        /// </summary>
        public static ComponentSystem ComponentSystem => componentSystem ?? (componentSystem = new ComponentSystem());

        /// <summary>
        /// 组件的根
        /// </summary>
        public static ComponentRoot ComponentRoot
        {
            get
            {
                if (componentRoot != null)
                {
                    return componentRoot;
                }

                Object.GameRoot = GameObject.Find("/GameRoot");

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

                Object.ParentNullRoot = new GameObject("DisposedObjectRoot");
                Object.ParentNullRoot.transform.SetParent(Object.GameRoot.transform, false);
                Object.ParentNullRoot.SetActive(false);

#endif

                componentRoot = new ComponentRoot();
                componentRoot.AddComponent<TimerComponent>();

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

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

            UnityEngine.Object.DestroyImmediate(Object.ParentNullRoot);
            Object.ParentNullRoot = null;
#endif

            Object.GameRoot = null;
        }
    }
}