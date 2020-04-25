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

            Init();
        }

        private void Init()
        {
            //添加初始场景上的组件 一般为通用组件 全局唯一等类型的组件
            //资源管理器
            AddComponent<ResourcesComponent>();
            //FairyGUI UI管理器
            AddComponent<FUIComponent>();
        }
    }
}