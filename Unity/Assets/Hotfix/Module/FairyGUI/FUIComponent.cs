namespace Hotfix
{
    internal class FUIComponent : Component, IAwakeSystem
    {
        public static FUIComponent Instance { get; private set; } = null;

        public void Awake()
        {
            Instance = this;

            Init();
        }

        private void Init()
        {
            FairyGUIHelper.Init();

            AddComponent<FUIPackagesComponent>();
        }
    }
}