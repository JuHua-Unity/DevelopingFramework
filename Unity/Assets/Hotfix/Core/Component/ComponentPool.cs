using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal class ComponentPool : Component
    {
        private readonly Dictionary<Type, Queue<Component>> dictionary = new Dictionary<Type, Queue<Component>>();

        public Component Fetch(Type type)
        {
            return Game.ObjectPool.Fetch(type) as Component;
        }

        public T Fetch<T>() where T : Component
        {
            T t = Game.ObjectPool.Fetch<T>();
            return t;
        }

        public void Recycle(Component obj)
        {
            obj.Parent = this;
            Game.ObjectPool.Recycle(obj);
        }
    }
}