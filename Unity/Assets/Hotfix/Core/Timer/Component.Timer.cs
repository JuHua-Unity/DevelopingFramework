using System;
using System.Linq;
using Async;
using Void = Async.Void;

namespace Hotfix
{
    internal partial class Component
    {
        protected void OnceWait(long time, Action run, int group = 0)
        {
            if (run == null)
            {
                return;
            }

            if (time <= 0)
            {
                run.Invoke();
                return;
            }

            OnceWaitAsync(time, run, group).Coroutine();
        }

        private async Void OnceWaitAsync(long time, Action run, int group)
        {
            if (await WaitAsync(time, group))
            {
                return;
            }

            run?.Invoke();
        }

        protected async Task<bool> WaitAsync(long time, int group = 0)
        {
            var c = AddMultiComponent<OnceWaitComponent, int>(group);
            var r = await c.WaitAsync(time);
            RemoveMultiComponent(c);
            return r;
        }

        protected void CancelWaitAsync(int group = 0)
        {
            var a = GetMultiComponents<OnceWaitComponent>();
            if (a == null)
            {
                return;
            }

            for (var i = 0; i < a.Length; i++)
            {
                var b = a[i];
                if (b.Group != group)
                {
                    continue;
                }

                b.Cancel();
            }
        }

        private void OnceWaitTimerDispose()
        {
            var a = GetMultiComponents<OnceWaitComponent>();
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
        protected void RepeatWait(int id, long time, int times, Action<int> run)
        {
            if (time <= 0)
            {
                throw new Exception($"RepeatWaitAsync time={time}");
            }

            var a = GetMultiComponents<RepeatWaitComponent>();
            if (a != null && a.Any(t => t.Id == id))
            {
                Error($"重新使用一个ID，id={id}已被使用！");
                return;
            }

            RepeatWaitAsync(id, time, times, run).Coroutine();
        }

        private async Void RepeatWaitAsync(int id, long time, int times, Action<int> run)
        {
            var c = AddMultiComponent<RepeatWaitComponent, int>(id);
            await c.WaitAsync(time, times, run);
            RemoveMultiComponent(c);
        }

        protected void StopRepeat(int id)
        {
            var a = GetMultiComponents<RepeatWaitComponent>();
            if (a != null)
            {
                for (var i = 0; i < a.Length; i++)
                {
                    var b = a[i];
                    if (b.Id != id)
                    {
                        continue;
                    }

                    b.Cancel();
                    return;
                }
            }

            Error($"id={id}并未使用，请检查！");
        }

        private void RepeatWaitTimerDispose()
        {
            var a = GetMultiComponents<RepeatWaitComponent>();
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
}