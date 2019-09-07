namespace Model
{
    internal class MonoStaticMethod : IStaticMethod
    {
        private readonly System.Reflection.MethodInfo methodInfo;
        private readonly object[] param;

        public MonoStaticMethod(System.Type type, string methodName)
        {
            methodInfo = type.GetMethod(methodName);
            param = new object[methodInfo.GetParameters().Length];
        }

        public override void Run()
        {
            methodInfo.Invoke(null, param);
        }

        public override void Run(object a)
        {
            param[0] = a;
            methodInfo.Invoke(null, param);
        }

        public override void Run(object a, object b)
        {
            param[0] = a;
            param[1] = b;
            methodInfo.Invoke(null, param);
        }

        public override void Run(object a, object b, object c)
        {
            param[0] = a;
            param[1] = b;
            param[2] = c;
            methodInfo.Invoke(null, param);
        }
    }
}