﻿using System;
using System.Collections.Generic;
using Async;

namespace Hotfix
{
    internal sealed class TimerComponent : Component, IUpdateSystem, IDestroySystem, IAwakeSystem
    {
        #region 实例

        public static TimerComponent Instance { get; private set; }

        public static bool Active => Instance != null;

        #endregion

        /// <summary>
        /// 正在等待的所有时间
        /// key:timeId 依次叠加来的
        /// </summary>
        private readonly Dictionary<long, TaskCompletionSource<bool>> timers = new Dictionary<long, TaskCompletionSource<bool>>();

        /// <summary>
        /// SortedDictionary:每次插入都会排序
        /// 正在等待的所有时间
        /// key:真实时间
        /// value:这个时间点对应的有哪些id(也就是timers的key)
        /// </summary>
        private readonly SortedDictionary<long, Queue<long>> timeId = new SortedDictionary<long, Queue<long>>();

        private readonly Queue<long> timeOutTime = new Queue<long>();
        private readonly Queue<long> timeOutTimerIds = new Queue<long>();
        private long minTime;

        public void Update()
        {
            if (this.timeId.Count == 0)
            {
                return;
            }

            var timeNow = TimeHelper.Now;

            if (timeNow < this.minTime)
            {
                return;
            }

            foreach (var kv in this.timeId)
            {
                var k = kv.Key;
                if (k > timeNow)
                {
                    this.minTime = k;
                    break;
                }

                this.timeOutTime.Enqueue(k);
            }

            while (this.timeOutTime.Count > 0)
            {
                var time = this.timeOutTime.Dequeue();
                var timerIds = this.timeId[time];
                foreach (var timerId in timerIds)
                {
                    this.timeOutTimerIds.Enqueue(timerId);
                }

                timerIds.Clear();
                Game.ObjectPool.Recycle_Queue_long(timerIds);
                this.timeId.Remove(time);
            }

            while (this.timeOutTimerIds.Count > 0)
            {
                var timerId = this.timeOutTimerIds.Dequeue();
                Remove(timerId, true);
            }
        }

        private void Remove(long tId, bool result = false)
        {
            if (!this.timers.TryGetValue(tId, out var timer))
            {
                return;
            }

            this.timers.Remove(tId);
            timer?.SetResult(!result);
        }

        public Task<bool> WaitTillAsync(long tillTime, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            var cid = GenerateId();
            this.timers[cid] = tcs;
            if (!this.timeId.ContainsKey(tillTime))
            {
                this.timeId.Add(tillTime, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[tillTime].Enqueue(cid);
            if (tillTime < this.minTime)
            {
                this.minTime = tillTime;
            }

            cancellationToken?.Register(() => { Remove(cid); });
            return tcs.Task;
        }

        public Task<bool> WaitTillAsync(long tillTime)
        {
            var tcs = new TaskCompletionSource<bool>();
            var cid = GenerateId();

            this.timers[cid] = tcs;
            if (!this.timeId.ContainsKey(tillTime))
            {
                this.timeId.Add(tillTime, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[tillTime].Enqueue(cid);
            if (tillTime < this.minTime)
            {
                this.minTime = tillTime;
            }

            return tcs.Task;
        }

        public Task<bool> WaitAsync(long time, CancellationToken cancellationToken)
        {
            var tillTime = TimeHelper.Now + time;
            var tcs = new TaskCompletionSource<bool>();
            var cid = GenerateId();
            this.timers[cid] = tcs;
            if (!this.timeId.ContainsKey(tillTime))
            {
                this.timeId.Add(tillTime, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[tillTime].Enqueue(cid);
            if (tillTime < this.minTime)
            {
                this.minTime = tillTime;
            }

            cancellationToken?.Register(() => { Remove(cid); });
            return tcs.Task;
        }

        public Task<bool> WaitAsync(long time)
        {
            var tillTime = TimeHelper.Now + time;
            var tcs = new TaskCompletionSource<bool>();
            var cid = GenerateId();
            this.timers[cid] = tcs;
            if (!this.timeId.ContainsKey(tillTime))
            {
                this.timeId.Add(tillTime, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[tillTime].Enqueue(cid);
            if (tillTime < this.minTime)
            {
                this.minTime = tillTime;
            }

            return tcs.Task;
        }

        #region 接口实现

        public void Awake()
        {
            if (Instance != null)
            {
                throw new Exception($"TimerComponent 实例已存在，请勿再次添加此组件！Parent={this.Parent?.GetType().FullName}");
            }

            Instance = this;
        }

        public void Destroy()
        {
            //优先设置为null 防止后面调用的时候继续可用
            Instance = null;

            foreach (var tId in this.timeId)
            {
                var longQueue = tId.Value;
                longQueue.Clear();
                Game.ObjectPool.Recycle_Queue_long(longQueue);
            }

            this.timeId.Clear();
            this.timeOutTimerIds.Clear();
            this.timeOutTime.Clear();

            foreach (var timer in this.timers)
            {
                timer.Value.SetResult(true);
            }

            this.timers.Clear();
        }

        #endregion

        #region 生成ID

        private static long id;

        private static long GenerateId()
        {
            return id++;
        }

        #endregion
    }
}