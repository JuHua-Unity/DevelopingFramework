using System;
using System.Collections.Generic;

namespace Hotfix
{
    /// <inheritdoc />
    /// <summary>
    /// 对象池
    /// </summary>
    internal sealed class ObjectPool : Object
    {
        private readonly Dictionary<Type, Queue<Object>> dictionary = new Dictionary<Type, Queue<Object>>();

        /// <summary>
        /// 抓取一个对象
        /// 设置该对象为 来自对象池
        /// </summary>
        /// <param name="type">对象类型 限制：type需为Object类型</param>
        /// <returns>该对象</returns>
        public Object Fetch(Type type)
        {
            Object obj;
            if (this.dictionary.TryGetValue(type, out var queue) && queue.Count > 0)
            {
                obj = queue.Dequeue();
                if (queue.Count == 0)
                {
                    RecycleObjectQueue(queue);
                    this.dictionary.Remove(type);
                }
            }
            else
            {
                obj = (Object) Activator.CreateInstance(type);
            }

            obj.IsFromPool = true;
            return obj;
        }

        /// <summary>
        /// 抓取一个对象
        /// 设置该对象为 来自对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>该对象</returns>
        public T Fetch<T>() where T : Object
        {
            var t = (T) Fetch(typeof(T));
            return t;
        }

        /// <summary>
        /// 归还对象到对象池
        /// </summary>
        /// <param name="obj">归还对象</param>
        public void Recycle(Object obj)
        {
            var type = obj.GetType();
            if (!this.dictionary.TryGetValue(type, out var queue))
            {
                queue = GetObjectQueue();
                this.dictionary.Add(type, queue);
            }

            obj.IsFromPool = false;
            queue.Enqueue(obj);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.dictionary.Clear();
            this.objectQueues.Clear();
            this.longQueues.Clear();
            this.componentLists.Clear();

            base.Dispose();
        }

        #region Object池子

        private readonly Queue<Queue<Object>> objectQueues = new Queue<Queue<Object>>();

        private Queue<Object> GetObjectQueue()
        {
            return this.objectQueues.Count > 0 ? this.objectQueues.Dequeue() : new Queue<Object>();
        }

        private void RecycleObjectQueue(Queue<Object> objQueue)
        {
            this.objectQueues.Enqueue(objQueue);
        }

        #endregion

        #region Queue<long>池子

        private readonly Queue<Queue<long>> longQueues = new Queue<Queue<long>>();

        public Queue<long> Fetch_Queue_long()
        {
            return this.longQueues.Count > 0 ? this.longQueues.Dequeue() : new Queue<long>();
        }

        public void Recycle_Queue_long(Queue<long> longQueue)
        {
            this.longQueues.Enqueue(longQueue);
        }

        #endregion

        #region List<Component>池子

        private readonly Queue<List<Component>> componentLists = new Queue<List<Component>>();

        public List<Component> Fetch_List_Component()
        {
            return this.componentLists.Count > 0 ? this.componentLists.Dequeue() : new List<Component>();
        }

        public void Recycle_List_Component(List<Component> componentList)
        {
            this.componentLists.Enqueue(componentList);
        }

        #endregion
    }
}