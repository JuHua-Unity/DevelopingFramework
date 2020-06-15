using System;
using System.Reflection;

namespace ILRuntime.Hotfix
{
    internal class MonoStaticMethod : IStaticMethod
    {
        private readonly MethodInfo methodInfo;
        private readonly object[] param;

        public MonoStaticMethod(Type type, string methodName)
        {
            this.methodInfo = type.GetMethod(methodName);
            if (this.methodInfo == null)
            {
                throw new Exception($"{type.Name}中没有{methodName}方法！");
            }

            this.param = new object[this.methodInfo.GetParameters().Length];
        }

        public override void Run()
        {
            this.methodInfo.Invoke(null, this.param);
        }

        public override void Run(object a)
        {
            this.param[0] = a;
            this.methodInfo.Invoke(null, this.param);
        }

        public override void Run(object a, object b)
        {
            this.param[0] = a;
            this.param[1] = b;
            this.methodInfo.Invoke(null, this.param);
        }

        public override void Run(object a, object b, object c)
        {
            this.param[0] = a;
            this.param[1] = b;
            this.param[2] = c;
            this.methodInfo.Invoke(null, this.param);
        }
    }
}