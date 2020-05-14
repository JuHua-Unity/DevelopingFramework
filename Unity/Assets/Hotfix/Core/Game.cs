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
#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
                objectPool.SetParent(Object.GameRoot);
#endif
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
#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
                componentSystem.SetParent(Object.GameRoot);
#endif
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

                if (Object.GameRoot == null)
                {
                    var a = new GameObject("GameRoot");
                    UnityEngine.Object.DontDestroyOnLoad(a);
                    Object.GameRoot = a;
                }

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
                if (Object.DisposedObjectsParent == null)
                {
                    var b = new GameObject("DisposedObjects");
                    b.transform.SetParent(Object.GameRoot.transform, false);
                    b.SetActive(false);
                    Object.DisposedObjectsParent = b;
                }

                if (Object.ObjectsParent == null)
                {
                    var c = new GameObject("Objects");
                    c.transform.SetParent(Object.GameRoot.transform, false);
                    Object.ObjectsParent = c;
                }

#endif

                componentRoot = ObjectPool.Fetch<ComponentRoot>();
#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
                componentRoot.SetParent(Object.GameRoot);
#endif
                return componentRoot;
            }
        }

        public static void Close()
        {
            if (componentRoot != null)
            {
                componentRoot.Dispose();
                componentRoot = null;
            }

            if (componentSystem != null)
            {
                componentSystem.Dispose();
                componentSystem = null;
            }

            objectPool?.Dispose();
            objectPool = null;

            //最后执行

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
            UnityEngine.Object.DestroyImmediate(Object.DisposedObjectsParent);
            Object.DisposedObjectsParent = null;

            UnityEngine.Object.DestroyImmediate(Object.ObjectsParent);
            Object.ObjectsParent = null;

#endif

            UnityEngine.Object.DestroyImmediate(Object.GameRoot);
            Object.GameRoot = null;
        }

        public static void Start()
        {
            GameStart.Start();
        }

        public static void ReStart()
        {
            if (componentRoot != null)
            {
                componentRoot.Dispose();
                componentRoot = null;
            }

            if (componentSystem != null)
            {
                componentSystem.Dispose();
                componentSystem = null;
            }

            GameStart.Start();
        }
    }
}