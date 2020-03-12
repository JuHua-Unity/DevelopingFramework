using Model;
using System.Collections.Generic;
using System.Threading;

namespace Hotfix
{
    internal sealed class TimerComponent : Component, IUpdateSystem, IDestroySystem
    {
        private readonly Dictionary<long, Timer> timers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, Queue<long>> timeId = new Dictionary<long, Queue<long>>();
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
                foreach (var timerId in this.timeId[time])
                {
                    this.timeOutTimerIds.Enqueue(timerId);
                }

                Game.ObjectPool.Recycle_Queue_long(this.timeId[time]);
                this.timeId.Remove(time);
            }

            while (this.timeOutTimerIds.Count > 0)
            {
                var timerId = this.timeOutTimerIds.Dequeue();
                if (!this.timers.TryGetValue(timerId, out var timer))
                {
                    continue;
                }

                Remove(timerId);
                timer.Tcs?.SetResult();
                RecycleTimer(timer);
            }
        }

        private void Remove(long tId)
        {
            this.timers.Remove(tId);
        }

        public Task WaitTillAsync(long tillTime, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            var timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;

            this.timers[timer.Id] = timer;
            if (!this.timeId.ContainsKey(timer.Time))
            {
                this.timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }

            cancellationToken.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitTillAsync(long tillTime)
        {
            var tcs = new TaskCompletionSource();
            var timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;

            this.timers[timer.Id] = timer;
            if (!this.timeId.ContainsKey(timer.Time))
            {
                this.timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }

            return tcs.Task;
        }

        public Task WaitAsync(long time, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            var timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;

            this.timers[timer.Id] = timer;
            if (!this.timeId.ContainsKey(timer.Time))
            {
                this.timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }

            cancellationToken.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitAsync(long time)
        {
            var tcs = new TaskCompletionSource();
            var timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;

            this.timers[timer.Id] = timer;
            if (!this.timeId.ContainsKey(timer.Time))
            {
                this.timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            this.timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }

            return tcs.Task;
        }

        #region Destroy

        public void Destroy()
        {
            for (var i = 0; i < this.timeId.Count; i++)
            {
                Game.ObjectPool.Recycle_Queue_long(this.timeId[i]);
            }

            this.timeId.Clear();
            this.timeOutTimerIds.Clear();
            this.timeOutTime.Clear();

            foreach (var timer in this.timers)
            {
                timer.Value.Dispose();
            }

            this.timers.Clear();
        }

        #endregion

        #region Timer

        private Timer GetTimer()
        {
            return ComponentFactory.Create<Timer>(this);
        }

        private void RecycleTimer(Timer timer)
        {
            timer.Dispose();
        }

        #region 生成ID

        private static long id;

        private static long GenerateId()
        {
            return id++;
        }

        #endregion

        private class Timer : Component
        {
            public long Id { get; set; }
            public long Time { get; set; }
            public TaskCompletionSource Tcs { get; set; }

            public override void Dispose()
            {
                if (this.IsDisposed)
                {
                    return;
                }

                base.Dispose();

                this.Id = 0;
                this.Time = 0;
                this.Tcs = null;
            }
        }

        #endregion
    }
}