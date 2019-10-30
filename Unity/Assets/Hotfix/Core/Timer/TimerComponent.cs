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
            if (timeId.Count == 0)
            {
                return;
            }

            long timeNow = TimeHelper.Now;

            if (timeNow < minTime)
            {
                return;
            }

            foreach (KeyValuePair<long, Queue<long>> kv in timeId)
            {
                long k = kv.Key;
                if (k > timeNow)
                {
                    minTime = k;
                    break;
                }

                timeOutTime.Enqueue(k);
            }

            while (timeOutTime.Count > 0)
            {
                long time = timeOutTime.Dequeue();
                foreach (long timerId in timeId[time])
                {
                    timeOutTimerIds.Enqueue(timerId);
                }

                Game.ObjectPool.Recycle_Queue_long(timeId[time]);
                timeId.Remove(time);
            }

            while (timeOutTimerIds.Count > 0)
            {
                long timerId = timeOutTimerIds.Dequeue();
                if (!timers.TryGetValue(timerId, out Timer timer))
                {
                    continue;
                }

                Remove(timerId);
                timer.Tcs?.SetResult();
                RecycleTimer(timer);
            }
        }

        private void Remove(long id)
        {
            timers.Remove(id);
        }

        public Task WaitTillAsync(long tillTime, CancellationToken cancellationToken)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            Timer timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }

            cancellationToken.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitTillAsync(long tillTime)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            Timer timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }

            return tcs.Task;
        }

        public Task WaitAsync(long time, CancellationToken cancellationToken)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            Timer timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }

            cancellationToken.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitAsync(long time)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            Timer timer = GetTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, Game.ObjectPool.Fetch_Queue_long());
            }

            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }

            return tcs.Task;
        }

        #region Destroy

        public void Destroy()
        {
            for (int i = 0; i < timeId.Count; i++)
            {
                Game.ObjectPool.Recycle_Queue_long(timeId[i]);
            }

            timeId.Clear();
            timeOutTimerIds.Clear();
            timeOutTime.Clear();

            foreach (var timer in timers)
            {
                timer.Value.Dispose();
            }

            timers.Clear();
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
                if (IsDisposed)
                {
                    return;
                }

                base.Dispose();

                Id = 0;
                Time = 0;
                Tcs = null;
            }
        }

        #endregion
    }
}