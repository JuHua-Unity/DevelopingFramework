using System;

namespace Hotfix
{
    public static class Init
    {
        public static void Start()
        {
            Log.Debug("热更启动完成！");

            Model.Game.Hotfix.Update = () => { Update(); };
            Model.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
            Model.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
            Model.Game.Hotfix.OnApplicationFocus = (focus) => { OnApplicationFocus(focus); };
            Model.Game.Hotfix.OnApplicationPause = (pause) => { OnApplicationPause(pause); };

            Model.Game.Hotfix.OnMessage = (id, obj) => { OnMessage(id, obj); };

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