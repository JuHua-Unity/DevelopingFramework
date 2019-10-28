namespace Editors
{
    public static class DrawerPriority
    {
        public const int String = -1;
        public const int Zero = 0;
        public const int Struct = 1;
        public const int Enum = 2;
        public const int Array = 3;
        public const int List = 4;
        public const int Dictionary = 5;
        public const int Object = int.MaxValue;
    }
}