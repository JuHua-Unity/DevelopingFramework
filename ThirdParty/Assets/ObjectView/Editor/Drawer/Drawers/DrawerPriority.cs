namespace ObjectDrawer
{
    internal static class DrawerPriority
    {
        public static int String { get; } = -1;
        public static int Zero { get; } = 0;
        public static int Enum { get; } = 2;
        public static int KeyValuePair { get; } = 3;
        public static int Collection { get; } = 4;
        public static int Delegate { get; } = 5;
        public static int Object { get; } = int.MaxValue;
    }
}