namespace Hotfix
{
    internal class LoadingScene : Component, IAwakeSystem
    {
        public static LoadingScene Instance { get; private set; } = null;

        public static void Open()
        {
            Game.ComponentRoot.AddComponent<LoadingScene>();
        }

        public void Awake()
        {
            Instance = this;

            //AddComponent<LoadingComponent>();
        }
    }
}