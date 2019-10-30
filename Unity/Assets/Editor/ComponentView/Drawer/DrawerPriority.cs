namespace Editors
{
    public static class DrawerPriority
    {
        public static int String { get; } = -1;

        public static int Zero { get; } = 0;

        public static int Struct { get; } = 10;

        public static int Queue { get; } = 20;
        public static int Stack { get; } = 20;
        public static int GenericQueue { get; } = 21;
        public static int GenericStack { get; } = 21;

        public static int Enum { get; } = 30;

        public static int Array { get; } = 50;
        public static int List { get; } = 51;
        public static int Dictionary { get; } = 52;
        public static int Collection { get; } = 53;

        public static int Delegate { get; } = 100;

        public static int Object { get; } = int.MaxValue;
    }
}