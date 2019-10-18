using System;
using System.Collections.Generic;

namespace Hotfix
{
    /// <summary>
    /// 组件基类
    /// 万物皆组件
    /// </summary>
    internal abstract class Component : Object
    {
        private Component parent;

        public Component Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public T GetParent<T>() where T : Component
        {
            return Parent as T;
        }

        #region 组件操作

        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public virtual K AddComponent<K>() where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K>(this, IsFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1>(P1 p1) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1>(this, p1, IsFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, IsFromPool);
            components.Add(type, component);
            return component;
        }

        public virtual K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : Component, new()
        {
            Type type = typeof(K);
            if (components.ContainsKey(type))
            {
                throw new Exception($"添加组件时该组件存在，id:{ObjId}，component:{type.Name}");
            }

            K component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, IsFromPool);
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

        #endregion

        #region override

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            //先释放所有挂在自己身上的Component
            foreach (var item in components)
            {
#if UNITY_EDITOR
                item.Value.Dispose();
#else
                try
                {
                    item.Value.Dispose();
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
#endif
            }

            components.Clear();

            //释放自己
            Game.ComponentSystem.Destroy(this);
            Game.ComponentSystem.Remove(ObjId);
            if (IsFromPool)
            {
                Game.ObjectPool.Recycle(this);
            }
        }

        #endregion
    }
}