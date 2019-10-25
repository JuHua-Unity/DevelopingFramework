using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal class TestScene : Component
    {
        private sbyte field1_1;
        public sbyte field1_2;
        private static sbyte field1_3;
        public static sbyte field1_4;

        private short field2_1;
        public short field2_2;
        private static short field2_3;
        public static short field2_4;

        private int field3_1;
        public int field3_2;
        private static int field3_3;
        public static int field3_4;

        private long field4_1;
        public long field4_2;
        private static long field4_3;
        public static long field4_4;

        private byte field5_1;
        public byte field5_2;
        private static byte field5_3;
        public static byte field5_4;

        private ushort field6_1;
        public ushort field6_2;
        private static ushort field6_3;
        public static ushort field6_4;

        private uint field7_1;
        public uint field7_2;
        private static uint field7_3;
        public static uint field7_4;

        private ulong field8_1;
        public ulong field8_2;
        private static ulong field8_3;
        public static ulong field8_4;

        private char field9_1;
        public char field9_2;
        private static char field9_3;
        public static char field9_4;

        private float field10_1;
        public float field10_2;
        private static float field10_3;
        public static float field10_4;

        private double field11_1;
        public double field11_2;
        private static double field11_3;
        public static double field11_4;

        private decimal field12_1;
        public decimal field12_2;
        private static decimal field12_3;
        public static decimal field12_4;

        private bool field13_1;
        public bool field13_2;
        private static bool field13_3;
        public static bool field13_4;

        private TestEnum1 field14_1;
        public TestEnum1 field14_2;
        private static TestEnum1 field14_3;
        public static TestEnum1 field14_4;

        private TestEnum2 field15_1;
        public TestEnum2 field15_2;
        private static TestEnum2 field15_3;
        public static TestEnum2 field15_4;

        private TestStruct field16_1;
        public TestStruct field16_2;
        private static TestStruct field16_3;
        public static TestStruct field16_4;


        public int[] array = new int[1] { 1 };
        public List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6 };
        public UnityEngine.AnimationCurve animationCurve;
        public UnityEngine.GameObject GameObject1 = GameRoot;
    }

    public enum TestEnum1
    {
        Enum1,
        Enum2
    }

    [Flags]
    public enum TestEnum2
    {
        Enum1 = 1,
        Enum2 = 2,
        Enum3 = 4,
        Enum4 = 8
    }

    public struct TestStruct
    {
        public int Field1;
        public string Field2;
    }
}