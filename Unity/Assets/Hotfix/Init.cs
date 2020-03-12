namespace Hotfix
{
    public static class Init
    {
        public static void Start()
        {
#if ILRuntime
            ILHelper.InitILRuntime();
#endif

            Model.Game.Hotfix.Update = Update;
            Model.Game.Hotfix.LateUpdate = LateUpdate;
            Model.Game.Hotfix.OnApplicationQuit = OnApplicationQuit;
            Model.Game.Hotfix.OnApplicationFocus = OnApplicationFocus;
            Model.Game.Hotfix.OnApplicationPause = OnApplicationPause;

            Model.Game.Hotfix.OnMessage = OnMessage;

            Log.Debug("热更启动完成！");
            GameStart.Start();
        }

        private static void OnMessage(int id, object obj)
        {
        }

        private static void Update()
        {
            Game.ComponentSystem.Update();
        }

        private static void LateUpdate()
        {
            Game.ComponentSystem.LateUpdate();
        }

        private static void OnApplicationQuit()
        {
            Game.Close();
        }

        private static void OnApplicationFocus(bool focus)
        {
        }

        private static void OnApplicationPause(bool pause)
        {
        }
    }
}