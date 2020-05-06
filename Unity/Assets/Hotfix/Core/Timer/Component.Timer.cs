using System;
using System.Collections.Generic;
using Async;
using Void = Async.Void;

namespace Hotfix
{
    internal partial class Component
    {
        protected async Task WaitAsync(long time, int waiter = 0)
        {
            if (!TimerComponent.Active)
            {
                return;
            }

            var c = AddSourceComponent(waiter);
            await TimerComponent.Instance.WaitAsync(time, c.Token);
            RemoveSourceComponent(c);
        }

        protected void CancelWaitAsync(int waiter = 0)
        {
            var a = GetMultiComponents<TimerCancellationTokenSource>();
            if (a == null)
            {
                return;
            }

            for (var i = 0; i < a.Length; i++)
            {
                var b = a[i];
                if (b.Waiter != waiter)
                {
                    continue;
                }

                a[i]?.Cancel();
            }
        }

        private TimerCancellationTokenSource AddSourceComponent(int waiter)
        {
            return AddMultiComponent<TimerCancellationTokenSource, int>(waiter);
        }

        private void RemoveSourceComponent(TimerCancellationTokenSource c)
        {
            if (!c.IsDisposed)
            {
                RemoveMultiComponent(c);
            }
        }

        private void TimerDispose()
        {
            var a = GetMultiComponents<TimerCancellationTokenSource>();
            if (a == null)
            {
                return;
            }

            for (var i = 0; i < a.Length; i++)
            {
                a[i]?.Cancel();
            }
        }
    }

    internal partial class Component
    {
        private List<int> repeatIds;

        protected void RepeatWaitAsync(int id, long time, int times, Action<int> run)
        {
            if (!TimerComponent.Active)
            {
                return;
            }

            if (time <= 0)
            {
                run?.Invoke(0);
                return;
            }

            if (!AddRepeatId(id))
            {
                Error($"重新使用一个ID，id={id}已被使用！");
                return;
            }

            WaitAsync(id, time, times, run).Coroutine();
        }

        protected void StopRepeat(int id)
        {
            if (!this.repeatIds.Contains(id))
            {
                Error($"id={id}并未使用，请检查！");
                return;
            }

            var a = GetMultiComponents<TimerCancellationTokenSource>();
            if (a == null)
            {
                return;
            }

            for (var i = 0; i < a.Length; i++)
            {
                var b = a[i];
                if (b.Waiter != id)
                {
                    continue;
                }

                a[i]?.Cancel();
            }
        }

        private async Void WaitAsync(int id, long time, int times, Action<int> run)
        {
            for (var i = 0; i < times; i++)
            {
                run?.Invoke(times - i);
                var c = AddSourceComponent(id);
                var cancel = await TimerComponent.Instance.WaitAsync(time, c.Token);
                RemoveSourceComponent(c);

                if (!cancel)
                {
                    continue;
                }

                //取消
                this.repeatIds.Remove(id);
                return;
            }

            run?.Invoke(0);
            this.repeatIds.Remove(id);
        }

        private bool AddRepeatId(int waiter)
        {
            if (this.repeatIds == null)
            {
                this.repeatIds = new List<int> {waiter};
                return true;
            }

            if (this.repeatIds.Contains(waiter))
            {
                return false;
            }

            this.repeatIds.Add(waiter);
            return true;
        }
    }

    internal partial class Component
    {
        private class TimerCancellationTokenSource : Component, IAwakeSystem<int>, IDestroySystem
        {
            private CancellationTokenSource Source;

            public int Waiter { get; private set; }
            public CancellationToken Token => this.Source?.Token;

            public void Awake(int a)
            {
                this.Waiter = a;
                this.Source = AddComponent<CancellationTokenSource>();
            }

            public void Destroy()
            {
                this.Source = null;
                this.Waiter = 0;
            }

            public void Cancel()
            {
                this.Source?.Cancel();
            }
        }
    }
}