using System;

namespace Test
{
    public class Test
    {
        public delegate void TestDelegate<T>();

        //public Action<Action<int>, Func<int, string, TestDelegate<bool>>> TestAction1;

        //public Action<int> TestAction2;
        //public Action<int, string> TestAction3;

        //public Func<int> TestFunc1;
        //public Func<string> TestFunc2;
        //public Func<bool> TestFunc3;
        //public Func<bool> TestFunc4;

        //public TestDelegate<int> TestDelegateInt;

        //public TestDelegate<bool> Get(Action<int> action, Func<int, string> func)
        //{
        //    action(1);
        //    func(2);
        //    return AAA;
        //}

        //public void Get<T>(Action<T> action)
        //{

        //}

        //private void AAA()
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class Test<T>
    {
        public void Get<T1>(Action<T, T1> action)
        {

        }
    }
}