using UnityEngine;

namespace Model
{
    internal class Init : MonoBehaviour
    {
        private void Start()
        {
            Log.Debug("启动游戏...");

            DontDestroyOnLoad(this);
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
            Game.Hotfix.OnApplicationQuit?.Invoke();
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