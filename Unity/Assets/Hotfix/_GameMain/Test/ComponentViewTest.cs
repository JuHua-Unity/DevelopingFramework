using System;
using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
    internal class ComponentViewTest : Component
    {
        #region 界面开关

        private static Component ParentComponent;

        public static void Open(Component parent = null)
        {
            if (parent == null)
            {
                ParentComponent = Game.ComponentRoot;
                Game.ComponentRoot.AddComponent<ComponentViewTest>();
            }
            else
            {
                ParentComponent = parent;
                parent.AddComponent<ComponentViewTest>();
            }
        }

        public static void Close()
        {
            ParentComponent.RemoveComponent<ComponentViewTest>();
            ParentComponent = null;
        }

        #endregion


        //private sbyte field1_1;
        //public sbyte field1_2;
        //private static sbyte field1_3;
        //public static sbyte field1_4;

        //private short field2_1;
        //public short field2_2;
        //private static short field2_3;
        //public static short field2_4;

        //private int field3_1;
        //public int field3_2;
        //private static int field3_3;
        //public static int field3_4;

        //private long field4_1;
        //public long field4_2;
        //private static long field4_3;
        //public static long field4_4;

        //private byte field5_1;
        //public byte field5_2;
        //private static byte field5_3;
        //public static byte field5_4;

        //private ushort field6_1;
        //public ushort field6_2;
        //private static ushort field6_3;
        //public static ushort field6_4;

        //private uint field7_1;
        //public uint field7_2;
        //private static uint field7_3;
        //public static uint field7_4;

        //private ulong field8_1;
        //public ulong field8_2;
        //private static ulong field8_3;
        //public static ulong field8_4;

        //private char field9_1;
        //public char field9_2;
        //private static char field9_3;
        //public static char field9_4;

        //private float field10_1;
        //public float field10_2;
        //private static float field10_3;
        //public static float field10_4;

        //private double field11_1;
        //public double field11_2;
        //private static double field11_3;
        //public static double field11_4;

        //private decimal field12_1;
        //public decimal field12_2;
        //private static decimal field12_3;
        //public static decimal field12_4;

        //private bool field13_1;
        //public bool field13_2;
        //private static bool field13_3;
        //public static bool field13_4;

        //private TestEnum1 field14_1;
        //public TestEnum1 field14_2;
        //private static TestEnum1 field14_3;
        //public static TestEnum1 field14_4;

        //private TestEnum2 field15_1;
        //public TestEnum2 field15_2;
        //private static TestEnum2 field15_3;
        //public static TestEnum2 field15_4;

        //private TestStruct field16_1;
        //public TestStruct field16_2;
        //private static TestStruct field16_3;
        //public static TestStruct field16_4;

        //private TestClass field17_1;
        //public TestClass field17_2;
        //private static TestClass field17_3;
        //public static TestClass field17_4;

        //private TestClass field18_1 = new TestClass();
        //public TestClass field18_2 = new TestClass();
        //private static TestClass field18_3 = new TestClass();
        //public static TestClass field18_4 = new TestClass();

        //private ITestInterface field19_1;
        //public ITestInterface field19_2;
        //private static ITestInterface field19_3;
        //public static ITestInterface field19_4;

        //private ITestInterface field20_1 = new TestInterface1();
        //public ITestInterface field20_2 = new TestInterface1();
        //private static ITestInterface field20_3 = new TestInterface1();
        //public static ITestInterface field20_4 = new TestInterface1();

        //private Testdelegate field21_1 = new Testdelegate(TestdelegateMethod);
        //public Testdelegate field21_2 = new Testdelegate(TestdelegateMethod);
        //private static Testdelegate field21_3 = new Testdelegate(TestdelegateMethod);
        //public static Testdelegate field21_4 = new Testdelegate(TestdelegateMethod);
        //private static void TestdelegateMethod() { Log.Debug("测试委托！"); }

        //private List<int> field22_5;

        //private List<int> field22_1 = new List<int>();
        //public List<int> field22_2 = new List<int>();
        //private static List<int> field22_3 = new List<int>();
        //public static List<int> field22_4 = new List<int>();

        //private List<string> field23_1 = new List<string>();
        //public List<string> field23_2 = new List<string>();
        //private static List<string> field23_3 = new List<string>();
        //public static List<string> field23_4 = new List<string>();

        //private List<TestClass> field24_1 = new List<TestClass>();
        //public List<TestClass> field24_2 = new List<TestClass>();
        //private static List<TestClass> field24_3 = new List<TestClass>();
        //public static List<TestClass> field24_4 = new List<TestClass>();

        //private TestClass[] field25_1 = new TestClass[0];
        //public TestClass[] field25_2 = new TestClass[0];
        //private static TestClass[] field25_3 = new TestClass[0];
        //public static TestClass[] field25_4 = new TestClass[0];

        //private TestClass[] field26_1 = new TestClass[1];
        //public TestClass[] field26_2 = new TestClass[1];
        //private static TestClass[] field26_3 = new TestClass[1];
        //public static TestClass[] field26_4 = new TestClass[1];

        //private Dictionary<int, string> field27_1 = new Dictionary<int, string>();
        //public Dictionary<int, string> field27_2 = new Dictionary<int, string>();
        //private static Dictionary<int, string> field27_3 = new Dictionary<int, string>();
        //public static Dictionary<int, string> field27_4 = new Dictionary<int, string>();

        //public Dictionary<int, string> field28_1 = new Dictionary<int, string>()
        //{
        //    {1,"1"},
        //    {2,"2"}
        //};
        //public Dictionary<TestClass, TestClass> field28_2 = new Dictionary<TestClass, TestClass>()
        //{
        //    {new TestClass(),new TestClass()},
        //    {new TestClass(),new TestClass()},
        //};

        //public Hashtable field29_1 = new Hashtable()
        //{
        //    {"1","1"},
        //    {1,1},
        //    {"2",2},
        //    {2,"2"},
        //    {new TestClass(),"TestClass"},
        //};

        //public TestClass[] field30_1 = new TestClass[10];
        //public Array field30_2 = Array.CreateInstance(typeof(TestClass), 10);
        //public readonly List<TestClass> field30_3 = new List<TestClass>();
        //public UnityEngine.GameObject[] field30_4 = new UnityEngine.GameObject[10];

        //public UnityEngine.GameObject[] field31_1 = new UnityEngine.GameObject[1];
        //public TestClass[] field31_2 = new TestClass[1];
        //public int[] field31_3 = new int[1];
        //public string[] field31_4 = new string[1];

        //public Queue field32_1 = null;
        //public Queue field32_2 = new Queue();
        //public Queue field32_3 = new Queue(new List<object>() { "1", new TestClass() });

        //public Stack field33_1 = null;
        //public Stack field33_2 = new Stack();
        //public Stack field33_3 = new Stack(new List<object>() { "1", new TestClass(), "2", new TestClass(), 1 });

        //public Stack<int> field34_1 = null;
        //public Stack<int> field34_2 = new Stack<int>();
        //public Stack<int> field34_3 = new Stack<int>(new List<int>() { 1, 2, 3 });

        //public Stack<TestClass> field35_1 = null;
        //public Queue<TestClass> field36_2 = null;
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

    [Model.NewObjectForComponentView]
    public struct TestStruct
    {
        public int Field1;
        public string Field2;
    }

    [Model.NewObjectForComponentView]
    public class TestClass
    {
        public int Field1;
        public string Field2;
        public int Field3 { get; set; }
        public string Field4 { get; set; }
    }

    public interface ITestInterface
    {
        int Field1 { get; }
        string Field2 { get; }
        void Method1();
        int Method2();
        string Method3();
    }

    public class TestInterface1 : ITestInterface
    {
        public int Field;

        public int Field1 => 10;

        public string Field2 => "10";

        public void Method1()
        {
        }

        public int Method2()
        {
            return 0;
        }

        public string Method3()
        {
            return "";
        }
    }

    public delegate void Testdelegate();
}