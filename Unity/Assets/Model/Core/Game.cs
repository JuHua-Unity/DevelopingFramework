namespace Model
{
    /// <summary>
    /// GameCore
    /// 框架核心
    /// </summary>
    public static class Game
    {
        private static Hotfix hotfix;
        private static Timer timer;
        private static Resources resources;
        private static Pool pool;

        public static Hotfix Hotfix
        {
            get
            {
                return hotfix ?? (hotfix = new Hotfix());
            }
        }

        public static Timer Timer
        {
            get
            {
                return timer ?? (timer = new Timer());
            }
        }

        public static Resources Resources
        {
            get
            {
                return resources ?? (resources = new Resources());
            }
        }

        public static Pool Pool
        {
            get
            {
                return pool ?? (pool = new Pool());
            }
        }

        public static void Close()
        {
            Hotfix.OnApplicationQuit?.Invoke();

        }
    }
}