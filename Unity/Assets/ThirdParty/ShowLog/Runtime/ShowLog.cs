namespace ShowLog
{
    public static class ShowLog
    {
        public static void Init()
        {
#if DEFINE_SHOWLOG
            ShowLogHelper.Open();
#endif
        }
    }
}