using Async;

namespace Hotfix
{
    internal partial class Component
    {
        private TimerComponent timer;

        private TimerComponent Timer => this.timer ?? (this.timer = Game.ComponentRoot.AddComponent<TimerComponent>());

        protected async Task WaitAsync(long time, int waiter = 0)
        {
            var c = AddMultiComponent<CancellationTokenSource>();
            await this.Timer.WaitAsync(time, c.Token);
            RemoveMultiComponent(c);
        }

        ////////////////////////取消没搬过来
    }
}