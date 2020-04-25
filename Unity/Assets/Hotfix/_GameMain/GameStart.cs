using System.Threading;
using Async;

namespace Hotfix
{
    internal class GameStart
    {
        public static void Start()
        {
            Log.Debug($"进入游戏主逻辑...");

            InitScene.Open();

            LoadingScene.Open();

            ComponentViewTest.Open();
            Log.Debug("1");
            //AsyncMethod().Coroutine();
        }

        private static async Void AsyncMethod()
        {
            Log.Debug("2");
            var t = InitScene.Instance.GetComponent<TimerComponent>();
            //if (t == null)
            //{
            //    Log.Error(4);
            //}
            //await t.WaitAsync(1000);
            //Log.Debug("3");
            ////new Test11().M();
            //await t.WaitAsync(1000);
            ////Model.Game.ReStart();
            var num = 0;
            while (num < 10)
            {
                num++;
                await t.WaitAsync(1000);
                Log.Debug($"num={num}");
            }

            Model.Game.ReStart();
        }
    }

    //public class Test11 : Model.ITest
    //{
    //    public void M()
    //    {
    //        Log.Debug("M");
    //    }
    //}
}