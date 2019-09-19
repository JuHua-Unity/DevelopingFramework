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
            Log.Debug("启动游戏...");

            GameStart().Coroutine();
        }

        private async Void GameStart()
        {
            GameStart g = new GameStart();
            await g.GameStartAsync(gameObject.Get<TextAsset>("LaunchOptions"));
        }

        private void Update()
        {
            Game.Hotfix.Update?.Invoke();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }

        private void OnApplicationFocus(bool focus)
        {
            Game.Hotfix.OnApplicationFocus?.Invoke(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            Game.Hotfix.OnApplicationPause?.Invoke(pause);
        }
    }
}