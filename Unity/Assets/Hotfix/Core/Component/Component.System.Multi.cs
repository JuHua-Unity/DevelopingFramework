using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal partial class Component
    {
        private readonly Dictionary<Type, List<Component>> multiComponents = new Dictionary<Type, List<Component>>();

        public virtual K AddMultiComponent<K>(bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            K component = ComponentFactory.Create<K>(this, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public virtual K AddMultiComponent<K, P1>(P1 p1, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1>(this, p1, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public virtual K AddMultiComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public virtual K AddMultiComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = true) where K : Component, new()
        {
            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public virtual void RemoveMultiComponents<K>() where K : Component
        {
            Type type = typeof(K);
            if (!multiComponents.TryGetValue(type, out List<Component> components))
            {
                return;
            }

            multiComponents.Remove(type);
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Dispose();
            }

            Game.ObjectPool.Recycle_List_Component(components);
        }

        public virtual void RemoveMultiComponents(Type type)
        {
            if (!multiComponents.TryGetValue(type, out List<Component> components))
            {
                return;
            }

            multiComponents.Remove(type);
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Dispose();
            }

            Game.ObjectPool.Recycle_List_Component(components);
        }

        public virtual void RemoveMultiComponent(Component component)
        {
            Type type = component.GetType();
            if (!multiComponents.TryGetValue(type, out List<Component> components))
            {
                return;
            }

            if (!components.Contains(component))
            {
                return;
            }

            components.Remove(component);
            component.Dispose();
            if (components.Count > 0)
            {
                return;
            }

            multiComponents.Remove(type);
            Game.ObjectPool.Recycle_List_Component(components);
        }

        public K[] GetMultiComponents<K>() where K : Component
        {
            if (!multiComponents.TryGetValue(typeof(K), out List<Component> components))
            {
                return default;
            }

            return (K[])(components.ToArray());
        }

        public Component[] GetMultiComponents(Type type)
        {
            if (!multiComponents.TryGetValue(type, out List<Component> components))
            {
                return null;
            }

            return components.ToArray();
        }
    }
}