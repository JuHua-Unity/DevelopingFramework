using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal partial class Component
    {
        private Dictionary<Type, List<Component>> multiComponents;

        /// <summary>
        /// 挂组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public K AddMultiComponent<K>(bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            var type = typeof(K);
            var component = ComponentFactory.Create<K>(this, isFromPool);
            if (!this.multiComponents.ContainsKey(type))
            {
                this.multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            this.multiComponents[type].Add(component);
            return component;
        }

        /// <summary>
        /// 挂组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <param name="p1"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public K AddMultiComponent<K, P1>(P1 p1, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            var type = typeof(K);
            var component = ComponentFactory.Create<K, P1>(this, p1, isFromPool);
            if (!this.multiComponents.ContainsKey(type))
            {
                this.multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            this.multiComponents[type].Add(component);
            return component;
        }

        /// <summary>
        /// 挂组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public K AddMultiComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            var type = typeof(K);
            var component = ComponentFactory.Create<K, P1, P2>(this, p1, p2, isFromPool);
            if (!this.multiComponents.ContainsKey(type))
            {
                this.multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            this.multiComponents[type].Add(component);
            return component;
        }

        /// <summary>
        /// 挂组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <typeparam name="P3"></typeparam>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public K AddMultiComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = true) where K : Component, new()
        {
            MultiSystemInit();

            var type = typeof(K);
            var component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3, isFromPool);
            if (!this.multiComponents.ContainsKey(type))
            {
                this.multiComponents.Add(type, Game.ObjectPool.Fetch_List_Component());
            }

            this.multiComponents[type].Add(component);
            return component;
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        public void RemoveMultiComponents<K>() where K : Component
        {
            RemoveMultiComponents(typeof(K));
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="type"></param>
        public void RemoveMultiComponents(Type type)
        {
            if (this.multiComponents == null)
            {
                Log.Error($"{GetType().Name}[{this.ObjId}]中并不存在任何类型的MultiComponents！");
                return;
            }

            if (!this.multiComponents.ContainsKey(type))
            {
                Log.Error($"{GetType().Name}[{this.ObjId}]中并不存在[{type.Name}]类型的MultiComponents！");
                return;
            }

            var multiComponent = this.multiComponents[type];
            for (var i = 0; i < multiComponent.Count; i++)
            {
                multiComponent[i].Dispose();
            }

            multiComponent.Clear();
            Game.ObjectPool.Recycle_List_Component(multiComponent);
            this.multiComponents.Remove(type);
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="component"></param>
        public void RemoveMultiComponent(Component component)
        {
            if (component == null)
            {
                throw new Exception("RemoveMultiComponent时Component参数为空！");
            }

            if (component.IsDisposed)
            {
                throw new Exception("RemoveMultiComponent时Component已被Dispose！");
            }

            if (this.multiComponents == null)
            {
                Log.Error($"{GetType().Name}[{this.ObjId}]中并不存在任何类型的MultiComponents！");
                return;
            }

            var type = component.GetType();
            if (!this.multiComponents.ContainsKey(type))
            {
                Log.Error($"{GetType().Name}[{this.ObjId}]中并不存在[{type.Name}]类型的MultiComponents！");
                return;
            }

            var multiComponent = this.multiComponents[type];
            if (!multiComponent.Contains(component))
            {
                Log.Error($"{GetType().Name}[{this.ObjId}]中并不存在[{type.Name}]类型的MultiComponent[{component.ObjId}]！");
                return;
            }

            multiComponent.Remove(component);
            component.Dispose();
            if (multiComponent.Count > 0)
            {
                return;
            }

            Game.ObjectPool.Recycle_List_Component(multiComponent);
            this.multiComponents.Remove(type);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public K[] GetMultiComponents<K>() where K : Component
        {
            return (K[]) GetMultiComponents(typeof(K));
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Component[] GetMultiComponents(Type type)
        {
            if (this.multiComponents != null && this.multiComponents.ContainsKey(type))
            {
                return this.multiComponents[type].ToArray();
            }

            return null;
        }

        private void MultiSystemInit()
        {
            if (this.multiComponents == null)
            {
                this.multiComponents = new Dictionary<Type, List<Component>>();
            }
        }

        private void MultiSystemDispose()
        {
            if (this.multiComponents == null)
            {
                return;
            }

            foreach (var item in this.multiComponents)
            {
                var componentList = item.Value;
                for (var i = 0; i < componentList.Count; i++)
                {
                    var component = componentList[i];
                    component.Dispose();
                }

                Game.ObjectPool.Recycle_List_Component(componentList);
            }

            this.multiComponents.Clear();
        }
    }
}