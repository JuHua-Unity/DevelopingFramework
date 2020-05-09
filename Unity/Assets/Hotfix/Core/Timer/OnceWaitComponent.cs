using Async;

namespace Hotfix
{
    internal class OnceWaitComponent : Component, IAwakeSystem<int>, IDestroySystem
    {
        private CancellationTokenSource Source;

        public int Group { get; private set; }

        public void Awake(int a)
        {
            this.Group = a;
            this.Source = AddComponent<CancellationTokenSource>();
        }

        public void Destroy()
        {
            this.Source = null;
            this.Group = 0;
        }

        public async Task<bool> WaitAsync(long time)
        {
            if (!TimerComponent.Active)
            {
                //Timer已经失效(可能是在关闭游戏过程中) 处理为取消
                return true;
            }

            return await TimerComponent.Instance.WaitAsync(time, this.Source?.Token);
        }

        public void Cancel()
        {
            this.Source?.Cancel();
        }
    }
}