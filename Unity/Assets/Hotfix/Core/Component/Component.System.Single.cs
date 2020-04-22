using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal partial class Component
    {
        private Dictionary<Type, Component> components;

        public virtual K AddComponent<K>(bool isFromPool = true) where K : Component, new()
        {
            SingleSystemInit();

            var type = typeof(K);
            if (this.components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{this.ObjId}，component:{type.Name}");
            }

            var component = ComponentFactory.Create<K>(this, isFromPool);
            this.components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1>(P1 p1, bool isFromPool = true) where K : Component, new()
        {
            SingleSystemInit();

            var type = typeof(K);
            if (this.components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{this.ObjId}，component:{type.Name}");
            }

            var component = ComponentFactory.Create<K, P1>(this, p1, isFromPool);
            this.components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = true) where K : Component, new()
        {
            SingleSystemInit();

            var type = typeof(K);
            if (this.components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{this.ObjId}，component:{type.Name}");
            }

            var component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, isFromPool);
            this.components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = true) where K : Component, new()
        {
            SingleSystemInit();

            var type = typeof(K);
            if (this.components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{this.ObjId}，component:{type.Name}");
            }

            var component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, isFromPool);
            this.components.Add(type, component);
            return component;
        }

        public virtual void RemoveComponent<K>() where K : Component
        {
            RemoveComponent(typeof(K));
        }

        public virtual void RemoveComponent(Type type)
        {
            if (!this.components.TryGetValue(type, out var component))
            {
                return;
            }

            this.components.Remove(type);
            component.Dispose();
        }

        public K GetComponent<K>() where K : Component
        {
            return (K) GetComponent(typeof(K));
        }

        public Component GetComponent(Type type)
        {
            if (this.components != null && this.components.ContainsKey(type))
            {
                return this.components[type];
            }

            return null;
        }

        private void SingleSystemInit()
        {
            if (this.components == null)
            {
                this.components = new Dictionary<Type, Component>();
            }
        }

        private void SingleSystemDispose()
        {
            if (this.components == null)
            {
                return;
            }

            foreach (var item in this.components)
            {
                var component = item.Value;
                component.Dispose();
            }

            this.components.Clear();
        }
    }
}