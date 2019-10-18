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

            new GameStart().Start(gameObject.Get<TextAsset>("LaunchOptions"));
        }

        private void Update()
        {
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