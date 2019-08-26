using System;

namespace Model
{
    /// <summary>
    /// 组件工厂类
    /// </summary>
    internal static class ComponentFactory
    {
        public static T CreateWithParent<T>(Component parent, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            component.Parent = parent;
            Game.EventSystem.Awake(component);
            return component;
        }

        public static T CreateWithParent<T, A>(Component parent, A a, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            component.Parent = parent;
            Game.EventSystem.Awake(component, a);
            return component;
        }

        public static T CreateWithParent<T, A, B>(Component parent, A a, B b, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            component.Parent = parent;
            Game.EventSystem.Awake(component, a, b);
            return component;
        }

        public static T CreateWithParent<T, A, B, C>(Component parent, A a, B b, C c, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            component.Parent = parent;
            Game.EventSystem.Awake(component, a, b, c);
            return component;
        }

        public static T Create<T>(bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            Game.EventSystem.Awake(component);
            return component;
        }

        public static T Create<T, A>(A a, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            Game.EventSystem.Awake(component, a);
            return component;
        }

        public static T Create<T, A, B>(A a, B b, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            Game.EventSystem.Awake(component, a, b);
            return component;
        }

        public static T Create<T, A, B, C>(A a, B b, C c, bool fromPool = true) where T : Component
        {
            Create(out T component, fromPool);
            Game.EventSystem.Awake(component, a, b, c);
            return component;
        }

        private static void Create<T>(out T component, bool fromPool = true) where T : Component
        {
            component = fromPool
                ? Game.ObjectPool.Fetch<T>()
                : (T)Activator.CreateInstance(typeof(T));
            Game.EventSystem.Add(component);
        }
    }
}