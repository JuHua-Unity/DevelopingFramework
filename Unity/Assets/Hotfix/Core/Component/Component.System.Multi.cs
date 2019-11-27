using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal partial class Component
    {
        private Dictionary<Type, List<Component>> multiComponents = null;

        public K AddMultiComponent<K>(bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            Type type = typeof(K);
            K component = ComponentFactory.Create<K>(this, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public K AddMultiComponent<K, P1>(P1 p1, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1>(this, p1, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public K AddMultiComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public K AddMultiComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            Type type = typeof(K);
            K component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, isFromPool);
            if (!multiComponents.ContainsKey(type))
            {
                multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            multiComponents[type].Add(component);
            return component;
        }

        public void RemoveMultiComponents<K>() where K : Component
        {
            RemoveMultiComponents(typeof(K));
        }

        public void RemoveMultiComponents(Type type)
        {
            if (multiComponents == null)
            {
                Log.Error($"{GetType().Name}[{ObjId}]中并不存在任何类型的MultiComponents！");
                return;
            }

            if (!multiComponents.ContainsKey(type))
            {
                Log.Error($"{GetType().Name}[{ObjId}]中并不存在[{type.Name}]类型的MultiComponents！");
                return;
            }

            var components = multiComponents[type];
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Dispose();
            }

            Game.ObjectPool.Recycle_List_Component(components);
            multiComponents.Remove(type);
        }

        public void RemoveMultiComponent(Component component)
        {
            if (component == null)
            {
                throw new Exception($"RemoveMultiComponent时Component参数为空！");
            }

            if (component.IsDisposed)
            {
                throw new Exception($"RemoveMultiComponent时Component已被Dispose！");
            }

            if (multiComponents == null)
            {
                Log.Error($"{GetType().Name}[{ObjId}]中并不存在任何类型的MultiComponents！");
                return;
            }

            Type type = component.GetType();
            if (!multiComponents.ContainsKey(type))
            {
                Log.Error($"{GetType().Name}[{ObjId}]中并不存在[{type.Name}]类型的MultiComponents！");
            }

            var components = multiComponents[type];
            if (!components.Contains(component))
            {
                Log.Error($"{GetType().Name}[{ObjId}]中并不存在[{type.Name}]类型的MultiComponent[{component.ObjId}]！");
                return;
            }

            components.Remove(component);
            component.Dispose();
            if (components.Count > 0)
            {
                return;
            }

            Game.ObjectPool.Recycle_List_Component(components);
            multiComponents.Remove(type);
        }

        public K[] GetMultiComponents<K>() where K : Component
        {
            return (K[])GetMultiComponents(typeof(K));
        }

        public Component[] GetMultiComponents(Type type)
        {
            if (multiComponents != null && multiComponents.ContainsKey(type))
            {
                return multiComponents[type].ToArray();
            }

            return null;
        }

        private void MultiSystemInit()
        {
            if (multiComponents == null)
            {
                multiComponents = new Dictionary<Type, List<Component>>();
            }
        }

        private void MultiSystemDispose()
        {
            if (multiComponents == null)
            {
                return;
            }

            foreach (var item in multiComponents)
            {
                RemoveMultiComponents(item.Key);
            }

            multiComponents.Clear();
        }
    }
}