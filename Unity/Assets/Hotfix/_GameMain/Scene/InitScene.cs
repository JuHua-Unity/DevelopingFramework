namespace Hotfix
{
    internal class InitScene : Component, IAwakeSystem
    {
        public static InitScene Instance { get; private set; } = null;

        public static void Open()
        {
            Game.ComponentRoot.AddComponent<InitScene>();
        }

        public void Awake()
        {
            Instance = this;

            AddComponent<TimerComponent>();
        }
    }
}