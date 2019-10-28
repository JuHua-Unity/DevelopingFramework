using Model;

namespace Hotfix
{
    internal class GameStart
    {
        public static void Start()
        {
            Log.Debug($"进入游戏主逻辑...");

            Game.ComponentRoot.AddComponent<ComponentViewTest>();
            //TimerComponent timer = Game.ComponentRoot.AddComponent<TimerComponent>();
            //Game.ComponentRoot.AddComponent<TestScene>().AddComponent<TestScene>(false);
            //Process(timer).Coroutine();
        }

        private static async Void Process(TimerComponent timer)
        {
            await timer.WaitAsync(10000);
            Game.ComponentRoot.RemoveComponent<TestScene>();
            await timer.WaitAsync(5000);
            Game.ComponentRoot.AddComponent<TestScene>().AddComponent<TestScene>(false);
        }
    }
}