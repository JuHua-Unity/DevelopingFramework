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
        public static ObjectPool ObjectPool
        {
            get
            {
                if (objectPool != null)
                {
                    return objectPool;
                }

                objectPool = new ObjectPool();
                objectPool.SetParent(Object.GameRoot);
                return objectPool;
            }
        }

        /// <summary>
        /// 组件的事件系统
        /// </summary>
        public static ComponentSystem ComponentSystem
        {
            get
            {
                if (componentSystem != null)
                {
                    return componentSystem;
                }

                componentSystem = ObjectPool.Fetch<ComponentSystem>();
                componentSystem.SetParent(Object.GameRoot);
                return componentSystem;
            }
        }

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

                var a = new GameObject("GameRoot");
                UnityEngine.Object.DontDestroyOnLoad(a);
                Object.GameRoot = a;

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

                var b = new GameObject("DisposedObjects");
                b.transform.SetParent(Object.GameRoot.transform, false);
                b.SetActive(false);
                Object.DisposedObjectsParent = b;

                var c = new GameObject("Objects");
                c.transform.SetParent(Object.GameRoot.transform, false);
                Object.ObjectsParent = c;

#endif

                componentRoot = ObjectPool.Fetch<ComponentRoot>();
                componentRoot.SetParent(Object.GameRoot);
                return componentRoot;
            }
        }

        public static void Close()
        {
            componentRoot?.Dispose();
            ObjectPool.Recycle(componentRoot);
            componentRoot = null;

            componentSystem?.Dispose();
            ObjectPool.Recycle(objectPool);
            componentSystem = null;

            objectPool?.Dispose();
            objectPool = null;

            //最后执行

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

            UnityEngine.Object.DestroyImmediate(Object.DisposedObjectsParent);
            Object.DisposedObjectsParent = null;

            UnityEngine.Object.DestroyImmediate(Object.ObjectsParent);
            Object.ObjectsParent = null;

#endif

            Object.GameRoot = null;
        }
    }
}