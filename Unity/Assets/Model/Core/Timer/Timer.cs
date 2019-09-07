using System.Collections.Generic;
using System.Threading;

namespace Model
{
    public sealed class Timer
    {
        private readonly Dictionary<long, OTimer> timers = new Dictionary<long, OTimer>();
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
                RecycleLongQueue(timeId[time]);
                timeId.Remove(time);
            }

            while (timeOutTimerIds.Count > 0)
            {
                long timerId = timeOutTimerIds.Dequeue();
                if (!timers.TryGetValue(timerId, out OTimer timer))
                {
                    continue;
                }
                Remove(timerId);
                timer.Tcs?.SetResult();
                timer.Tcs_T?.SetResult(timer.CancellationTokenSource);
                RecycleOTimer(timer);
            }
        }

        private void Remove(long id)
        {
            timers.Remove(id);
        }

        public Task WaitTillAsync(long tillTime, CancellationTokenSource cancellationTokenSource)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;
            timer.CancellationTokenSource = cancellationTokenSource;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            timer.CancellationTokenSource.Token.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task<CancellationTokenSource> WaitTillAsyncWithCancel(long tillTime)
        {
            TaskCompletionSource<CancellationTokenSource> tcs = new TaskCompletionSource<CancellationTokenSource>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs_T = tcs;
            timer.CancellationTokenSource = cancellationTokenSource;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            timer.CancellationTokenSource.Token.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitTillAsync(long tillTime)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = tillTime;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            return tcs.Task;
        }

        public Task WaitAsync(long time, CancellationTokenSource cancellationTokenSource)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;
            timer.CancellationTokenSource = cancellationTokenSource;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            timer.CancellationTokenSource.Token.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task<CancellationTokenSource> WaitAsyncWithCancel(long time)
        {
            TaskCompletionSource<CancellationTokenSource> tcs = new TaskCompletionSource<CancellationTokenSource>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs_T = tcs;
            timer.CancellationTokenSource = cancellationTokenSource;
            timers[timer.Id] = timer;

            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            timer.CancellationTokenSource.Token.Register(() => { Remove(timer.Id); });
            return tcs.Task;
        }

        public Task WaitAsync(long time)
        {
            TaskCompletionSource tcs = new TaskCompletionSource();
            OTimer timer = GetOTimer();
            timer.Id = GenerateId();
            timer.Time = TimeHelper.Now + time;
            timer.Tcs = tcs;

            timers[timer.Id] = timer;
            if (!timeId.ContainsKey(timer.Time))
            {
                timeId.Add(timer.Time, GetLongQueue());
            }
            timeId[timer.Time].Enqueue(timer.Id);
            if (timer.Time < minTime)
            {
                minTime = timer.Time;
            }
            return tcs.Task;
        }

        #region OTimer

        private static readonly Queue<OTimer> oTimers = new Queue<OTimer>();

        private OTimer GetOTimer()
        {
            if (oTimers.Count > 0)
            {
                return oTimers.Dequeue();
            }

            return new OTimer();
        }

        private void RecycleOTimer(OTimer timer)
        {
            timer.Id = 0;
            timer.Time = 0;
            timer.Tcs = null;
            timer.Tcs_T = null;
            timer.CancellationTokenSource?.Cancel();
            timer.CancellationTokenSource = null;

            oTimers.Enqueue(timer);
        }

        #region 生成ID

        private static long id;

        private static long GenerateId()
        {
            return id++;
        }

        #endregion

        private class OTimer
        {
            public long Id { get; set; }
            public long Time { get; set; }
            public TaskCompletionSource Tcs { get; set; }
            public TaskCompletionSource<CancellationTokenSource> Tcs_T { get; set; }
            public CancellationTokenSource CancellationTokenSource { get; set; }
        }

        #endregion

        #region LongQueue

        private readonly Queue<Queue<long>> longQueues = new Queue<Queue<long>>();

        private Queue<long> GetLongQueue()
        {
            if (longQueues.Count > 0)
            {
                return longQueues.Dequeue();
            }
            return new Queue<long>();
        }

        private void RecycleLongQueue(Queue<long> longQueue)
        {
            longQueue.Clear();
            longQueues.Enqueue(longQueue);
        }

        #endregion
    }
}