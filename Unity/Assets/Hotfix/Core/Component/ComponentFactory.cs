using System;

namespace Hotfix
{
    /// <summary>
    /// 组件工厂类
    /// </summary>
    internal static class ComponentFactory
    {
        public static T Create<T>(Component parent, bool fromPool = true) where T : Component
        {
            CreateComponent(out T component, fromPool);
            component.Parent = parent;
            Game.ComponentSystem.Awake(component);
            return component;
        }

        public static T Create<T, A>(Component parent, A a, bool fromPool = true) where T : Component
        {
            CreateComponent(out T component, fromPool);
            component.Parent = parent;
            Game.ComponentSystem.Awake(component, a);
            return component;
        }

        public static T Create<T, A, B>(Component parent, A a, B b, bool fromPool = true) where T : Component
        {
            CreateComponent(out T component, fromPool);
            component.Parent = parent;
            Game.ComponentSystem.Awake(component, a, b);
            return component;
        }

        public static T Create<T, A, B, C>(Component parent, A a, B b, C c, bool fromPool = true) where T : Component
        {
            CreateComponent(out T component, fromPool);
            component.Parent = parent;
            Game.ComponentSystem.Awake(component, a, b, c);
            return component;
        }

        private static void CreateComponent<T>(out T component, bool fromPool = true) where T : Component
        {
            component = fromPool
                ? Game.ObjectPool.Fetch<T>()
                : (T)Activator.CreateInstance(typeof(T));
            Game.ComponentSystem.Add(component);
        }
    }
}