using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// 对象池
    /// </summary>
    public sealed class Pool
    {
        private readonly Dictionary<Type, Queue<object>> dictionary = new Dictionary<Type, Queue<object>>();

        /// <summary>
        /// 抓取一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>该对象</returns>
        public object Fetch(Type type)
        {
            object obj = null;
            if (dictionary.TryGetValue(type, out Queue<object> queue) && queue.Count > 0)
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
                obj = Activator.CreateInstance(type);
            }

            return obj;
        }

        /// <summary>
        /// 抓取一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>该对象</returns>
        public T Fetch<T>() where T : class
        {
            T t = (T)Fetch(typeof(T));
            return t;
        }

        /// <summary>
        /// 归还对象到对象池
        /// </summary>
        /// <param name="obj">归还对象</param>
        public void Recycle(object obj)
        {
            Type type = obj.GetType();
            if (!dictionary.TryGetValue(type, out Queue<object> queue))
            {
                queue = GetObjectQueue();
                dictionary.Add(type, queue);
            }

            queue.Enqueue(obj);
        }

        public void Dispose()
        {
            foreach (var kv in dictionary)
            {
                RecycleObjectQueue(kv.Value);
            }

            dictionary.Clear();
        }

        #region ObjectQueue

        private readonly Queue<Queue<object>> objQueues = new Queue<Queue<object>>();

        private Queue<object> GetObjectQueue()
        {
            if (objQueues.Count > 0)
            {
                return objQueues.Dequeue();
            }

            return new Queue<object>();
        }

        private void RecycleObjectQueue(Queue<object> objQueue)
        {
            objQueue.Clear();
            objQueues.Enqueue(objQueue);
        }

        #endregion

        #region Generic

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

        #region Non Generic



        #endregion
    }
}