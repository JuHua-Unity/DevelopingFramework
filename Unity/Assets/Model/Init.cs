using UnityEngine;

namespace Model
{
    internal class Init : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Log.Debug($"UnityVersion:{Application.unityVersion}\tAppVersion:{Application.version}\t游戏名:{Application.productName}\tPlatform:{Application.platform}");

            Game.StartProcess = 1;
        }

        private void Update()
        {
            if (Game.StartProcess == 1)
            {
                //启动
                new GameStart().Start(gameObject.Get<TextAsset>("LaunchOptions"));
                Game.StartProcess = 0;
                return;
            }

            if (Game.StartProcess == 2)
            {
                //GC回收
                System.GC.Collect();
                Game.StartProcess = 1;
                return;
            }

            if (Game.StartProcess == 3)
            {
                //关闭热更层
                Game.Close();
                Game.StartProcess = 2;
                return;
            }

            if (Game.Hotfix == null)
            {
                return;
            }

            Game.Hotfix.Update?.Invoke();
        }

        private void LateUpdate()
        {
            if (Game.Hotfix == null)
            {
                return;
            }

            Game.Hotfix.LateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (Game.Hotfix == null)
            {
                return;
            }

            Game.Hotfix.OnApplicationFocus?.Invoke(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            if (Game.Hotfix == null)
            {
                return;
            }

            Game.Hotfix.OnApplicationPause?.Invoke(pause);
        }
    }
}