namespace Hotfix
{
    public static class Init
    {
        public static void Start()
        {
            Log.Debug("启动热更...");

            Model.Game.Hotfix.Update = () => { Update(); };
            Model.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
            Model.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
            Model.Game.Hotfix.OnApplicationFocus = (focus) => { OnApplicationFocus(focus); };
            Model.Game.Hotfix.OnApplicationPause = (pause) => { OnApplicationPause(pause); };
        }

        private static void Update()
        {

        }

        private static void LateUpdate()
        {

        }

        private static void OnApplicationQuit()
        {

        }

        private static void OnApplicationFocus(bool focus)
        {

        }

        private static void OnApplicationPause(bool pause)
        {

        }
    }
}