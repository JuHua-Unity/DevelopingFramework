using System;
using System.Threading;
using ReferenceCollector;
using UnityEngine;

namespace Model
{
    internal class Init : MonoBehaviour
    {
        private void Start()
        {
            Log.Debug($"UnityVersion:{Application.unityVersion}\tAppVersion:{Application.version}\t游戏名:{Application.productName}\tPlatform:{Application.platform}\tThreadId:{Thread.CurrentThread.ManagedThreadId}");

            Game.Start();
        }

        private void Update()
        {
            switch (Game.StartProcess)
            {
                case 4:
                    Game.Hotfix?.Update?.Invoke();
                    break;
                case 1:
                    Game.StartProcess = -1;
                    //启动
                    new GameStart().Start(this.gameObject.Get<TextAsset>("LaunchOptions"));
                    Game.StartProcess = 4;
                    return;
                case 2:
                    Game.StartProcess = -2;
                    //GC回收
                    GC.Collect();
                    Game.StartProcess = 1;
                    return;
                case 3:
                    Game.StartProcess = -3;
                    //关闭热更层
                    Game.Close();
                    Game.StartProcess = 2;
                    return;
            }
        }

        private void LateUpdate()
        {
            Game.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.OnApplicationQuit();
        }

        private void OnApplicationFocus(bool focus)
        {
            Game.OnApplicationFocus(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            Game.OnApplicationPause(pause);
        }
    }
}