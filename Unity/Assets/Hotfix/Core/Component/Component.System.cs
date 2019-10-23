using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal partial class Component
    {
        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public virtual K AddComponent<K>(bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K>(this, isFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1>(P1 p1, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1>(this, p1, isFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, isFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, isFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual void RemoveComponent<K>() where K : Component
        {
            if (IsDisposed)
            {
                return;
            }

            Type type = typeof(K);
            if (!components.TryGetValue(type, out Component component))
            {
                return;
            }

            components.Remove(type);
            component.Dispose();
        }

        public virtual void RemoveComponent(Type type)
        {
            if (IsDisposed)
            {
                return;
            }

            if (!components.TryGetValue(type, out Component component))
            {
                return;
            }

            components.Remove(type);
            component.Dispose();
        }

        public K GetComponent<K>() where K : Component
        {
            if (!components.TryGetValue(typeof(K), out Component component))
            {
                return default;
            }

            return (K)component;
        }

        public Component GetComponent(Type type)
        {
            if (!components.TryGetValue(type, out Component component))
            {
                return null;
            }

            return component;
        }
    }
}