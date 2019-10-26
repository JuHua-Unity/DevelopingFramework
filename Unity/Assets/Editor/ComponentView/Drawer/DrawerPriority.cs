namespace Editors
{
    public static class DrawerPriority
    {
        public const int Zero = 0;
        public const int String = -1;
        public const int Struct = 10;
        public const int Enum = 20;
        public const int List = 30;
        public const int Dictionary = 40;
        public const int Object = int.MaxValue;
    }
}