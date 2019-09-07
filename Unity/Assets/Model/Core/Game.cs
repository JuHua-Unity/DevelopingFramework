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

        public static void Close()
        {

        }
    }
}