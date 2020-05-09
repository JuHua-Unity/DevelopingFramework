using System;
using Async;

namespace Hotfix
{
    internal class RepeatWaitComponent : Component, IAwakeSystem<int>, IDestroySystem
    {
        public int Id { get; private set; }

        private long wait_Time;
        private Action<int> wait_CallBack;
        private int wait_CurTimes; //当前是第几次 已经执行了该值减一次
        private int wait_Times;
        private bool wait_Cancel; //手动取消的时候 如果是发生在回调函数里面的话将取消不掉 所以添加此变量 不能作为其他判断使用

        public void Awake(int a)
        {
            this.Id = a;
        }

        public void Destroy()
        {
            this.Id = 0;
            this.wait_Time = 0;
            this.wait_CurTimes = 0;
            this.wait_Times = 0;
            this.wait_CallBack = null;
            this.wait_Cancel = false;
        }

        public async Task<bool> WaitAsync(long time, int times, Action<int> run)
        {
            this.wait_Time = time;
            this.wait_Times = times;
            this.wait_CallBack = run;

            return await WaitAsync();
        }

        public void Cancel()
        {
            this.wait_Cancel = true;
            var a = GetMultiComponents<OnceWaitComponent>();
            if (a == null)
            {
                return;
            }

            for (var i = 0; i < a.Length; i++)
            {
                var b = a[i];
                if (b.Group != this.Id)
                {
                    continue;
                }

                b.Cancel();
            }
        }

        private async Task<bool> WaitAsync()
        {
            if (this.wait_Cancel)
            {
                return true;
            }

            for (var i = 0; i < this.wait_Times; i++)
            {
                this.wait_CurTimes = i + 1;
                var c = AddMultiComponent<OnceWaitComponent, int>(this.Id);
                var cancel = await c.WaitAsync(this.wait_Time);
                RemoveMultiComponent(c);

                if (cancel)
                {
                    //取消
                    //不执行回调
                    return true;
                }

                this.wait_CallBack?.Invoke(this.wait_CurTimes);
                if (this.wait_Cancel)
                {
                    return true;
                }
            }

            return false;
        }
    }
}