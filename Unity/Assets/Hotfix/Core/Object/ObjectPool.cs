﻿using System;
using System.Collections.Generic;

namespace Hotfix
{
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
            Object obj = null;
            if (dictionary.TryGetValue(type, out Queue<Object> queue) && queue.Count > 0)
            {
                obj = queue.Dequeue();
                if (queue.Count == 0)
                {
                    RecycleObjectQueue(queue);
                    dictionary.Remove(type);
                }
            }
            else
            {
                obj = (Object)Activator.CreateInstance(type);
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
            T t = (T)Fetch(typeof(T));
            return t;
        }

        /// <summary>
        /// 归还对象到对象池
        /// </summary>
        /// <param name="obj">归还对象</param>
        public void Recycle(Object obj)
        {
            Type type = obj.GetType();
            if (!dictionary.TryGetValue(type, out Queue<Object> queue))
            {
                queue = GetObjectQueue();
                dictionary.Add(type, queue);
            }

            queue.Enqueue(obj);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            dictionary.Clear();
            objQueues.Clear();
            longQueues.Clear();
        }

        #region ObjectQueue

        private readonly Queue<Queue<Object>> objQueues = new Queue<Queue<Object>>();

        private Queue<Object> GetObjectQueue()
        {
            if (objQueues.Count > 0)
            {
                return objQueues.Dequeue();
            }
            return new Queue<Object>();
        }

        private void RecycleObjectQueue(Queue<Object> objQueue)
        {
            objQueue.Clear();
            objQueues.Enqueue(objQueue);
        }

        #endregion

        #region Queue<Queue<long>>

        private readonly Queue<Queue<long>> longQueues = new Queue<Queue<long>>();

        public Queue<long> Fetch_Queue_long()
        {
            if (longQueues.Count > 0)
            {
                return longQueues.Dequeue();
            }

            return new Queue<long>();
        }

        public void Recycle_Queue_long(Queue<long> longQueue)
        {
            longQueue.Clear();
            longQueues.Enqueue(longQueue);
        }

        #endregion
    }
}