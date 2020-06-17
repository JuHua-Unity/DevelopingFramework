namespace Hotfix
{
    public static class Init
    {
        public static void Start()
        {
#if ILRuntime
            ILHelper.InitILRuntime();
#endif

            Model.GameEntry.Hotfix.LateUpdateAction= Update;
            Model.GameEntry.Hotfix.LateUpdateAction = LateUpdate;
            Model.GameEntry.Hotfix.OnApplicationQuitAction = OnApplicationQuit;
            Model.GameEntry.Hotfix.OnApplicationFocusAction = OnApplicationFocus;
            Model.GameEntry.Hotfix.OnApplicationPauseAction = OnApplicationPause;

            Model.GameEntry.Hotfix.OnMessage = OnMessage;

            Log.Debug("热更启动完成！");
            Game.Start();
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