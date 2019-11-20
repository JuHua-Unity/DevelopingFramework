namespace Hotfix
{
    internal class GameStart
    {
        public static void Start()
        {
            Log.Debug($"进入游戏主逻辑...");

            InitScene.Open();

            LoadingScene.Open();

            ComponentViewTest.Open();
            Game.ComponentRoot.AddComponent<CollectDelegate>();
        }
    }
}